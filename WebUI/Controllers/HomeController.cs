using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

// [Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private static Process? videoProcess;
    private int requestCount = 0;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://10.42.0.41:5000/flask/");
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<ActionResult> SendProximity(string speed)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(speed), "speed");
        try
        {
            var response = await _httpClient.PostAsync("set/servo", formData);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent == "1")
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Json(false);
    }

    [HttpPost]
    public async Task<ActionResult> SendGas(string gasSpeed)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(gasSpeed), "speed");
        try
        {
            var response = await _httpClient.PostAsync("set/servo", formData);

            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (responseContent == "1")
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Json(false);
    }

    [HttpPost]
    public IActionResult StartRecording()
    {
        if (videoProcess == null)
        {
            string command = "ffmpeg";
            string arguments = "-f mjpeg -r 24 -i \"http://10.42.0.41:8000/stream.mjpg\" -r 24 ./video.avi";

            // Yeni bir subprocess oluştur
            ProcessStartInfo startInfo = new ProcessStartInfo
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
}
