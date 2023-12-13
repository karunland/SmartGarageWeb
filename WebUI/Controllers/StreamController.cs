using System.Diagnostics;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers;

public class StreamController : Controller
{
    private static Process? videoProcess = null;
    private readonly FileService _fileService;
    private static Process? photoProcess;
    private readonly IConfiguration _configuration;
    private string Ip;

    public StreamController(FileService fileService, IConfiguration configuration)
    {
        _fileService = fileService;
        _configuration = configuration;
        Ip = _configuration["IpAdress"];
    }

    [HttpPost]
    public IActionResult StartRecording()
    {
        if (videoProcess != null)
            return Ok(new ApiResult
            {
                IsSuccess = false,
                Message = "Kayıt zaten başlatılmış"
            });

        string command = "ffmpeg";
        var fileName = _fileService.GetNewVideoFileName();
        string arguments = $"-f mjpeg -r 24 -i \"{Ip}:8000/stream.mjpg\" -r 24 ./Media/{fileName}";

        // Yeni bir subprocess oluştur
        var startInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        videoProcess = Process.Start(startInfo);
        return Ok(new ApiResult
        {
            IsSuccess = true,
            Message = "Kayıt başlatıldı"
        });
    }

    [HttpPost]
    public IActionResult StopRecording()
    {
        if (videoProcess == null || videoProcess.HasExited)
            return Ok(new ApiResult
            {
                IsSuccess = false,
                Message = "Kayıt başlatılmamış"
            });

        videoProcess.Kill();
        videoProcess = null;
        return Ok(new ApiResult
        {
            IsSuccess = true,
            Message = "Kayıt durduruldu"
        });
    }

    [HttpGet]
    public IActionResult CheckRecordingStatus()
    {
        if (videoProcess != null && !videoProcess.HasExited)
        {
            return Ok(new ApiResult
            {
                IsSuccess = true,
                Message = "Kayıt devam ediyor"
            });
        }
        else
        {
            return Ok(new ApiResult
            {
                IsSuccess = false,
                Message = "Kayıt durdu"
            });
        }
    }

    [HttpPost]
    public IActionResult CapturePhoto()
    {
        try
        {
            if (photoProcess != null && !photoProcess.HasExited)
            {
                photoProcess.Kill();
            }
            var fileName = _fileService.GetNewPhotoFileName();
            string ffmpegArgs = $"-i  \"{Ip}:8000/stream.mjpg\"  -vframes 1 -f image2 ./Media/{fileName}";

            ProcessStartInfo startInfo = new()
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            photoProcess = new Process();
            photoProcess.StartInfo = startInfo;
            photoProcess.Start();
            photoProcess.WaitForExit();
            int exitCode = photoProcess.ExitCode;
            if (exitCode != 0)
                return Ok(new ApiResult
                {
                    IsSuccess = false,
                    Message = "Fotoğraf çekilemedi"
                });
    
            var latestPhoto = _fileService.GetLatestPhoto();

            return Ok(new ApiResult<CapturePhotoResponse>
            {
                IsSuccess = true,
                Data = new CapturePhotoResponse
                {
                    FileName = latestPhoto
                }
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
}
