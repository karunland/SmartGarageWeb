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

    public FileController(FileService fileService)
    {
        _fileService = fileService;
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
    public async  Task<IActionResult> SendMail(MailModel mailModel)
    {
        try
        {
            string fullFilePath = Path.Combine(MediaFolderPath, mailModel.FileName+".png");

            if (!System.IO.File.Exists(fullFilePath))
            {
                return Ok(new ApiResult
                {
                    IsSuccess = false,
                    Message = "Dosya bulunamadı"
                });
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullFilePath);

            var file = Convert.ToBase64String(fileBytes);

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("karunlander@gmail.com", "yTYkztXNq5A6a9q"),
                EnableSsl = true
            };

            // client.SendAsync("mail", mailModel.MailAddress, "Güvenlik Kamerası", "Güvenlik Kamerası", file);

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
