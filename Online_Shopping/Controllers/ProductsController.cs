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

namespace Cloud_FinalTwo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Proudct_Qaunt,Artist,ProductPrice,ProductDescription,ProductAvailibility,ImageURL,ProductCategory")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Proudct_Qaunt,Artist,ProductPrice,ProductDescription,ProductAvailibility,ImageURL,ProductCategory")] Products products)
        {
            if (id != products.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductID))
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
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Remove associated cart items
            var cartItems = _context.Cart.Where(c => c.ProductID == id);
            _context.Cart.RemoveRange(cartItems);

            // Remove the product itself
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
       
        [Authorize]
        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            var username = User.Identity.Name; // Get the logged-in user's username



            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.ProductID == id && c.CartUser == username);

            if (cart == null)
            {
                // Add new cart item
                cart = new Cart
                {
                    ProductID = product.ProductID,
                    Product = product,
                    Name = product.ProductName,
                    Quantity = 1,
                    Price = Math.Round(product.ProductPrice, 2),
                    Artist = product.Artist,
                    CartUser = username // Set the CartUser to the logged-in user's username
                };

                _context.Cart.Add(cart);
            }
            else
            {
                // Update existing cart item
                cart.Quantity++;
                cart.Price = Math.Round((double)(product.ProductPrice * cart.Quantity), 2);
                _context.Cart.Update(cart);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Carts");
        }
        //Search Feature hopefully 
        public async Task<IActionResult> Search(string searchString)
        {
            // Query the database to find products matching the search string
            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString) || p.ProductCategory.Contains(searchString));
            }

            // Return the view with the filtered list of products
            return View(await products.ToListAsync());
        }
    }
}
