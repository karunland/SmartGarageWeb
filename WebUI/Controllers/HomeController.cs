using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private string Ip;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        Ip = _configuration["IpAdress"];

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(Ip + ":5000/flask/")
        };
    }

    public IActionResult Index()
    {
        ViewBag.Ip = Ip;
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> OpenDoor(string speed)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(speed), "speed");
        try
        {
            var response = await _httpClient.PostAsync("set/servo-open", formData);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent == "1")
                {
                    return Ok(new ApiResult
                    {
                        IsSuccess = true,
                        Message = "Kapı Açıldı"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Ok(new ApiResult
        {
            IsSuccess = false,
            Message = "Kapı Açılamadı"
        });
    }

    [HttpPost]
    public async Task<ActionResult> CloseDoor()
    {
        try
        {
            var response = await _httpClient.PostAsync("set/just-close", null);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent == "1")
                {
                    return Ok(new ApiResult
                    {
                        IsSuccess = true,
                        Message = "Kapı Kapatıldı"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Ok(new ApiResult
        {
            IsSuccess = false,
            Message = "Kapı Kapatılamadı"
        });
    }

    [HttpPost]
    public async Task<ActionResult> SendGas(string gasSpeed)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(gasSpeed), "speed");
        try
        {
            var response = await _httpClient.PostAsync("set/fan", formData);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return Ok(new ApiResult
                {
                    IsSuccess = true,
                    Message = responseContent
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Ok(new ApiResult
        {
            IsSuccess = false,
            Message = "Fan hızı değiştirilemedi"
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
