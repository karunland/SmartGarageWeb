using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers;

public class StreamController : Controller
{
    private static Process? videoProcess;
    private readonly FileService _fileService;
    private static Process? photoProcess;

    public StreamController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public IActionResult StartRecording()
    {
        if (videoProcess == null)
        {
            string command = "ffmpeg";
            var fileName = _fileService.GetNewVideoFileName();
            string arguments = $"-f mjpeg -r 24 -i \"http://10.42.0.41:8000/stream.mjpg\" -r 24 ./videos/{fileName}";

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
            return Json("True");
        }
        else
        {
            return Json("False");
        }
    }

    [HttpPost]
    public IActionResult StopRecording()
    {
        if (videoProcess != null && !videoProcess.HasExited)
        {
            videoProcess.Kill();
            videoProcess = null;
            return Json("True");
        }
        else
        {
            return Json("False");
        }
    }

    [HttpGet]
    public IActionResult CheckRecordingStatus()
    {
        if (videoProcess != null && !videoProcess.HasExited)
        {
            return Json("True");
        }
        else
        {
            return Json("False");
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

            string ffmpegArgs = "-i  \"http://10.42.0.41:8000/stream.mjpg\"  -vframes 1 -f image2 photo.png";

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
                return Json("False");
            return Json("True");
        }
        catch (Exception ex)
        {
            return Json("False");
        }
    }
}
