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
    [Authorize(Roles = "Admin")]
    public class ProccessingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProccessingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Proccessings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proccessing.ToListAsync());
        }

        // GET: Proccessings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proccessing = await _context.Proccessing
                .FirstOrDefaultAsync(m => m.Process_ID == id);
            if (proccessing == null)
            {
                return NotFound();
            }

            return View(proccessing);
        }

        // GET: Proccessings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proccessings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Process_ID,shopperID,pocessedProducts,processedTotal,numberItems,isProcessed,OrderDate")] Proccessing proccessing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proccessing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proccessing);
        }

        // GET: Proccessings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proccessing = await _context.Proccessing.FindAsync(id);
            if (proccessing == null)
            {
                return NotFound();
            }
            return View(proccessing);
        }

        // POST: Proccessings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Process_ID,shopperID,pocessedProducts,processedTotal,numberItems,isProcessed,OrderDate")] Proccessing proccessing)
        {
            if (id != proccessing.Process_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proccessing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProccessingExists(proccessing.Process_ID))
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
            return View(proccessing);
        }

        // GET: Proccessings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proccessing = await _context.Proccessing
                .FirstOrDefaultAsync(m => m.Process_ID == id);
            if (proccessing == null)
            {
                return NotFound();
            }

            return View(proccessing);
        }

        // POST: Proccessings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proccessing = await _context.Proccessing.FindAsync(id);
            if (proccessing != null)
            {
                _context.Proccessing.Remove(proccessing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProccessingExists(int id)
        {
            return _context.Proccessing.Any(e => e.Process_ID == id);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(int? processId)
        {
            if (processId == null)
            {
                return NotFound();
            }

            var processedOrder = await _context.Proccessing.FirstOrDefaultAsync(p => p.Process_ID == processId);

            if (processedOrder == null)
            {
                return NotFound();
            }

            // Create a new order from the processed order
            var order = new Orders
            {
                emailOrder = processedOrder.shopperID,
                ordered_Products = processedOrder.pocessedProducts,
                order_total = processedOrder.processedTotal,
                order_Qaunt = processedOrder.numberItems,
                isProcessed = true,
                OrderDate = processedOrder.OrderDate
            };

            _context.Orders.Add(order);

            // Mark the processed order as processed
            processedOrder.isProcessed = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Proccessings");
        }

    }
}
