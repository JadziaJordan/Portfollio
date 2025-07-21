using System.Diagnostics; // Provides tools for debugging and diagnostics
using Microsoft.AspNetCore.Mvc; // Required for MVC controllers and routing
using MathApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Imports the models from the MathApp project
using System.Net.Http;
using System.Net.Http.Json;

namespace MathApp.Controllers // Defines the namespace for the controller
{
    public class MathController : Controller
    {
         private readonly HttpClient _httpClient;

        public MathController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5206/api/Math/"); 
        }


    public IActionResult Calculate()
{
    List<SelectListItem> operations = new List<SelectListItem> {
        new SelectListItem { Value = "1", Text = "+" },
        new SelectListItem { Value = "2", Text = "-" },
        new SelectListItem { Value = "3", Text = "**" },
        new SelectListItem { Value = "4", Text = "/" },

        };

    ViewBag.Operations = operations;

    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Calculate(decimal? FirstNumber, decimal? SecondNumber,int Operation)
{
    var token = HttpContext.Session.GetString("currentUser");
    if (token == null)
    {
        return RedirectToAction("Login", "Auth");
    }

    MathCalculation calc;
    try
    {
        calc = MathCalculation.Create(FirstNumber, SecondNumber, Operation, 0, token);
    }
    catch (Exception ex)
    {
        ViewBag.Error = ex.Message;
        return View();
    }

    var response = await _httpClient.PostAsJsonAsync("PostCalculate", calc);

    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadFromJsonAsync<MathCalculation>();
        ViewBag.Result = result?.Result;
    }
    else
    {
        var errorContent = await response.Content.ReadAsStringAsync();
ViewBag.Error = "API Error: " + errorContent;
    }

    return View();
}



[HttpGet]
public async Task<IActionResult> History()
{
    string? token = HttpContext.Session.GetString("currentUser");

    if (string.IsNullOrEmpty(token))
    {
        return RedirectToAction("Login", "Auth");
    }

    var response = await _httpClient.GetAsync($"GetHistory?token={token}");

    if (response.IsSuccessStatusCode)
    {
        var history = await response.Content.ReadFromJsonAsync<List<MathCalculation>>();
        return View(history);
    }

    ViewBag.Error = "Could not fetch history.";
    return View(new List<MathCalculation>());
}


       public async Task<IActionResult> Clear()
        {
            var token = HttpContext.Session.GetString("currentUser");

            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            HttpResponseMessage response = await _httpClient.DeleteAsync("/api/Math/DeleteHistory?token=" + token);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("History");
        }



}

}