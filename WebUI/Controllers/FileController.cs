using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers;

public class FileController : Controller
{
    private const string MediaFolderPath = "./Media";
    private readonly FileService _fileService;
    private readonly IConfiguration _configuration;

    public FileController(FileService fileService, IConfiguration configuration)
    {
        _fileService = fileService;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetFile([FromQuery] string filePath)
    {
        try
        {
            string fullFilePath = Path.Combine(MediaFolderPath, filePath);

            if (!System.IO.File.Exists(fullFilePath))
            {
                return NotFound("Dosya bulunamadı");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullFilePath);

            return File(fileBytes, "image/png");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }


    [HttpPost]
    public async Task<IActionResult> SendMail(MailModel mailModel)
    {
        try
        {
            string fullFilePath = Path.Combine(MediaFolderPath, mailModel.FileName + ".png");

            if (!System.IO.File.Exists(fullFilePath))
            {
                return Ok(new ApiResult
                {
                    IsSuccess = false,
                    Message = "Dosya bulunamadı"
                });
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullFilePath);

            var senderEmail = _configuration["SmtpSettings:SenderEmail"];
            var password = _configuration["SmtpSettings:Password"];
            var receiver = mailModel.MailAddress;

            using (var message = new MailMessage(senderEmail, receiver)
            {
                Subject = "Smart Garage - Güvenli Kamerası",
                Body = "Fotoğrafınız ektedir."
            })
            {
                using (var ms = new MemoryStream(fileBytes))
                {
                    message.Attachments.Add(new Attachment(ms, mailModel.FileName + ".png"));
                    using (var client = new SmtpClient())
                    {
                        client.EnableSsl = false;
                        client.Credentials = new NetworkCredential(senderEmail, password);
                        client.Host = _configuration["SmtpSettings:Host"];
                        client.Port = int.Parse(_configuration["SmtpSettings:Port"]);

                        await client.SendMailAsync(message);
                    }
                }
            }

            return Ok(new ApiResult
            {
                IsSuccess = true,
                Message = "Mail gönderildi"
            });
        }
        catch (Exception ex)
        {
            return Ok(new ApiResult
            {
                IsSuccess = false,
                Message = ex.Message
            });
        }
    }


    [HttpGet]
    public IActionResult ReloadEvents()
    {
        return ViewComponent("Files");
    }
}
