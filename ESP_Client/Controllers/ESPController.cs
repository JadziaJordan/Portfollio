using ESP_Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ESP_Client.Controllers
{
    public class ESPController : Controller
    {
        // HttpClient instance to send requests to the ESP API
        private static HttpClient espClient = new()
        {
            BaseAddress = new Uri("https://developer.sepush.co.za/business/2.0/"),
        };

        // GET: ESP/Index
        // Displays the search page where users can input an area name
        public IActionResult Index()
        {
            return View();
        }

        // POST: ESP/Index done after the page loads 
        // Handles the form submission from the Index view.
        // It sends a GET request to the ESP API to search for areas matching the user input.
        [HttpPost]
        public async Task<IActionResult> Index(string search)
        {
            // Ensure the request contains the API token before sending the request
            if (!espClient.DefaultRequestHeaders.Contains("token"))
            {
                espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
            }

            // Send a GET request to the ESP API with the user-provided search text
            HttpResponseMessage response = await espClient.GetAsync("areas_search?text=" + search);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into an AreasModel object
            AreasModel? deserialisedResponse = JsonSerializer.Deserialize<AreasModel>(jsonResponse);

            // Return the view with the search results
            return View(deserialisedResponse);

            // TODO: Implement error handling for different response statuses (e.g., 200, 404, 403, 429, 500)
        }

        // GET: ESP/Details
        // Retrieves detailed information about a specific area based on the provided ID.
        public async Task<IActionResult> Details(string id)
        {
            // Ensure the request contains the API token before sending the request
            if (!espClient.DefaultRequestHeaders.Contains("token"))
            {
                espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
            }

            // Send a GET request to retrieve details for the specified area
            HttpResponseMessage response = await espClient.GetAsync("area?id=" + id);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into an EspResponse object
            EspResponse? deserialisedResponse = JsonSerializer.Deserialize<EspResponse>(jsonResponse);

            // Return the view with the area details
            return View(deserialisedResponse);

            // TODO: Implement error handling for different response statuses (e.g., 200, 404, 403, 429, 500)
        }
    }
}
