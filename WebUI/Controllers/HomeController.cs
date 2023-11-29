using System.Diagnostics;
using System.Text;
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
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://127.0.0.1:5000/flask/");
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
        try
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(speed), "speed");

            var response = await _httpClient.PostAsync("set/servo", formData);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        catch (Exception ex)
        {
            return Json(false);
        }

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> SendGas(string gasSpeed)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(gasSpeed), "speed");

            var response = await _httpClient.PostAsync("set/servo", formData);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        catch (Exception ex)
        {
            return Json(false);
        }

        return View();
    }
}