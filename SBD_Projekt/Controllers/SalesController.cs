using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDProjekt.Infrastructure;
using SBDProjekt.Models;
using SBDProjekt.Models.ViewModels;

namespace SBD_Projekt.Controllers
{
    public class SalesController : Controller
    {
        private readonly MyDBContext _context;

        public SalesController(MyDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sales.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.DiscountedProducts)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sale == null)
            {
                return NotFound();
            }
            
            var saleDetails = new DetailsSaleViewModel(sale);

            saleDetails.Discounts = await _context.DiscountedProduct
                .Where(s => s.SaleId == sale.Id).ToListAsync();

            return View(saleDetails);
        }

        // GET: Sales/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                var conflictingSales = IsTimeAvailable(sale.StartDate, sale.EndDate);

                if (conflictingSales != null)
                {
                    ViewBag.DateUnavailable = conflictingSales;
                    return View(sale);
                }

                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate")] Sale sale)
        {
            if (id != sale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var conflictingSales = IsTimeAvailable(sale.StartDate, sale.EndDate);

                    if (conflictingSales != null)
                    {
                        ViewBag.DateUnavailable = conflictingSales;
                        return View(sale);
                    }

                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.Id))
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
            return View(sale);
        }

        // GET: Sales/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }

        /// <summary>
        /// Checks wheter time between dates is available in database
        /// </summary>
        /// <returns>Null if time between is available or Sale object which has conflicts</returns>
        private IList<Sale> IsTimeAvailable(DateTime startDate, DateTime endDate)
        {
            var conflictingSales = _context.Sales
                .Where(s => (
                    (s.StartDate >= startDate && s.StartDate <= endDate) 
                    || (s.EndDate >= startDate && s.EndDate <= endDate)))
                .ToList();

            return conflictingSales == null || conflictingSales.Count == 0 ? null : conflictingSales;
        }
    }
}
