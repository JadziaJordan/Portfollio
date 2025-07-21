using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ESP_Client.Models;
using System.Text.Json;

namespace ESP_Client.Controllers;

public class HomeController : Controller
{
    // Logger instance for logging messages, warnings, and errors.
    private readonly ILogger<HomeController> _logger;

    // HttpClient instance for making API requests to the ESP API.
    private static HttpClient espClient = new()
    {
        BaseAddress = new Uri("https://developer.sepush.co.za/business/2.0/"),
    };

    // Constructor to initialize the logger.
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET: Home/Index
    public async Task<IActionResult> Index()
    {
        // Ensure the API token is included in the request headers.
        if (!espClient.DefaultRequestHeaders.Contains("token"))
        {
            espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
        }

        // Make a GET request to retrieve API allowance information.
        HttpResponseMessage response = await espClient.GetAsync("api_allowance");
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize the JSON response into an AllowanceResponse object.
        AllowanceResponse? deserialisedResponse = JsonSerializer.Deserialize<AllowanceResponse>(jsonResponse);

        // Pass the allowance details to the view.
        return View(deserialisedResponse.allowance);
    }

    // GET: Home/Privacy
    public IActionResult Privacy()
    {
        return View();
    }

    // Error handler for displaying error pages.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
