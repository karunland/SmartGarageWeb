using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://10.42.0.41:5000/flask/")
        };
    }

    public IActionResult Index()
    {
        return View();
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
                    return Ok(new ApiResult
                    {
                        IsSuccess = true,
                        Message = "Servo hareket ettirildi"
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
            Message = "Servo hareket ettirilemedi"
        });
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
                        return Ok(new ApiResult
                        {
                            IsSuccess = true,
                            Message = "Fan hareket ettirildi"
                        });
                    }
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
            Message = "Fan hareket ettirilemedi"
        });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
