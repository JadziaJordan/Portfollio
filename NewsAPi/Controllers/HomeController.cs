using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsAPi.Models;
using System.Text.Json;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private static HttpClient newsClient = new()
    {
        BaseAddress = new Uri("https://newsapi.org/v2/"),
    };

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        newsClient.DefaultRequestHeaders.Add("User-Agent", "NewsApi/1.0");
    }

    public async Task<IActionResult> Index()
    {
        string apiKey = Environment.GetEnvironmentVariable("AuthNewsKey");

        // Fetch news data from the API
        HttpResponseMessage response = await newsClient.GetAsync($"top-headlines?country=us&apiKey={apiKey}");
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
//debug response
        _logger.LogInformation($"API Response: {jsonResponse}");

        ArticleResponses? deserializedResponse = JsonSerializer.Deserialize<ArticleResponses>(jsonResponse, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (deserializedResponse == null)
        {
            _logger.LogError("Failed to deserialize the API response.");
            return View("Error");
        }

        return View(deserializedResponse);
    }
        // GET: Display the Search form
    public IActionResult Search()
    {
        return View();  // This will render the Search view with the form
    }

    // New Search Action
    [HttpPost]
    public async Task<IActionResult> Search(string category)
    {
        string apiKey = Environment.GetEnvironmentVariable("AuthNewsKey");

        // Ensure a category is provided
        if (string.IsNullOrEmpty(category))
        {
            _logger.LogError("Category is required for search.");
            return View("Error");
        }

        // Fetch news articles based on category
        HttpResponseMessage response = await newsClient.GetAsync($"everything?q={category}&sortBy=popularity&apiKey={apiKey}");

        var jsonResponse = await response.Content.ReadAsStringAsync();

        _logger.LogInformation($"Search API Response: {jsonResponse}");

        ArticleResponses? deserializedResponse = JsonSerializer.Deserialize<ArticleResponses>(jsonResponse, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (deserializedResponse == null)
        {
            _logger.LogError("Failed to deserialize the API response.");
            return View("Error");
        }

        return View(deserializedResponse);  // Return the search results view
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
