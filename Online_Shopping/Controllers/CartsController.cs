using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cloud_FinalTwo.Data;
using Cloud_FinalTwo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud_FinalTwo.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name; // Get the current user's username
            await StoreTotalCartPriceInSession(username);
            var applicationDbContext = _context.Cart
                                               .Include(c => c.Product)
                                               .Where(c => c.CartUser == username); // Filter by current user
            return View(await applicationDbContext.ToListAsync());
        }

        private async Task StoreTotalCartPriceInSession(string username)
        {
            // Filter the Cart items by the given username
            var totalPrice = await _context.Cart
                                           .Where(c => c.CartUser == username)
                                           .SumAsync(c => c.Price);
            var roundedTotalPrice = Math.Round((decimal)totalPrice, 2);
            TempData["TotalPrice"] = "R " + roundedTotalPrice.ToString();
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID");
            return View();
        }

        // POST: Carts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,CartUser,ProductID,Name,Quantity,Price,Artist")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", cart.ProductID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", cart.ProductID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("CartId,CartUser,ProductID,Name,Quantity,Price,Artist")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", cart.ProductID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int? id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

        // this should search for producst move it to process and the hopefully clears

        [HttpPost]
        public async Task<IActionResult> ProcessOrder()
        {
            var CustName = User.Identity.Name;
            await StoreTotalCartPriceInSession(CustName);


            var cartItems = await _context.Cart
                                           .Where(c => c.CartUser == CustName)
                                           .ToListAsync();


            var products = cartItems.Select(c => c.Name).ToList();


            var totalPrice = TempData["TotalPrice"] as string;
            var totalQuantity = cartItems.Sum(c => c.Quantity);


            var process = new Proccessing
            {
                shopperID = CustName,
                pocessedProducts = string.Join(",", products),
                processedTotal = totalPrice,
                numberItems = totalQuantity,
                isProcessed = false,
                OrderDate = DateTime.Now,
            };

            // Add order to the database
            _context.Proccessing.Add(process);
            await _context.SaveChangesAsync();

            // Clear cart that have been purchased
            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }
    }

}
