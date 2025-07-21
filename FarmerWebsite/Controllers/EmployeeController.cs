using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_PartTwo.Data;
using Prog7311_PartTwo.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Prog7311_PartTwo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public EmployeeController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // STEP 1: Show user registration form
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        // Handle user registration and assign Farmer role
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Ensure "Farmer" role exists
                    if (!await _roleManager.RoleExistsAsync("Farmer"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                    }

                    // Assign role
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    // Redirect to profile creation
                    return RedirectToAction("CreateProfile", "Employee", new { userId = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // Show profile creation form
        [HttpGet]
        public IActionResult CreateProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing");
            }

            var model = new FarmerProfileModel { UserId = userId };
            return View(model);
        }

        //  Handle profile submission and save to DB
        [HttpPost]
        public async Task<IActionResult> CreateProfile(FarmerProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _context.FarmerProfiles.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewAllFarmers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        //  View all farmer profiles
        [HttpGet]
        public IActionResult ViewAllFarmers()
        {
            var farmers = _context.FarmerProfiles
                .Include(f => f.User)
                .ToList();

            return View(farmers);
        }
    }
}
