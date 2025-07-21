using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_PartTwo.Data;
using Prog7311_PartTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prog7311_PartTwo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private void PopulateCategoryDropdown()
        {
            ViewBag.Categories = new List<string>
            {
                "Food",
                "Flowers",
                "Maize",
                "Equipment",
                "Vegetables",
                "Fruit",
            };
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            PopulateCategoryDropdown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductsModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategoryDropdown();

                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        ModelState.AddModelError(string.Empty, $"Field: {key} - Error: {error.ErrorMessage}");
                    }
                }

                ModelState.AddModelError(string.Empty, "Model state is invalid.");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                PopulateCategoryDropdown();
                ModelState.AddModelError(string.Empty, "Unable to retrieve logged-in user.");
                return View(model);
            }

            var farmerProfile = await _context.FarmerProfiles
                .FirstOrDefaultAsync(fp => fp.UserId == user.Id);

            if (farmerProfile == null)
            {
                PopulateCategoryDropdown();
                ModelState.AddModelError(string.Empty, "Farmer profile not found.");
                return View(model);
            }

            model.UserId = user.Id;
            model.Farmer = farmerProfile.FullName;

            try
            {
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewAllMyProducts");
            }
            catch (Exception ex)
            {
                PopulateCategoryDropdown();
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllMyProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var farmerProfile = await _context.FarmerProfiles
                .FirstOrDefaultAsync(fp => fp.UserId == user.Id);

            if (farmerProfile != null)
                ViewBag.FarmerName = farmerProfile.FullName;

            var products = await _context.Products
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllProducts(string category, string farmerName, DateTime? startDate, DateTime? endDate)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                products = products.Where(p => p.ProductCategory.Contains(category));

            if (!string.IsNullOrWhiteSpace(farmerName))
                products = products.Where(p => p.Farmer == farmerName);

            if (startDate.HasValue)
                products = products.Where(p => p.Production >= startDate.Value);

            if (endDate.HasValue)
                products = products.Where(p => p.Production <= endDate.Value);

            var result = await products.ToListAsync();

            var farmers = await _context.FarmerProfiles
                .Select(fp => fp.FullName)
                .Distinct()
                .ToListAsync();

            ViewBag.FarmerList = farmers;
            ViewBag.FarmerName = farmerName;
            ViewBag.Category = category;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(result);
        }
    }
}
