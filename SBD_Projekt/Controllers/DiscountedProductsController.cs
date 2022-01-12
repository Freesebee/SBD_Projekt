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
    [Authorize(Roles ="Admin")]
    public class DiscountedProductsController : Controller
    {
        private readonly MyDBContext _context;

        public DiscountedProductsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: DiscountedProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountedProduct = await _context.DiscountedProduct
                .Include(d => d.Product)
                .Include(d => d.Sale)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (discountedProduct == null)
            {
                return NotFound();
            }

            return View(discountedProduct);
        }

        // GET: DiscountedProducts/Create
        public async Task<IActionResult> CreateAsync(int? productId)
        {
            if (productId == null)
                return NotFound("Product not found");

            EditDiscountedProductViewModel model = new EditDiscountedProductViewModel();
            model.SaleList = await _context.Sales.ToListAsync();
            model.ProductId = (int)productId;

            return View(model);
        }

        // POST: DiscountedProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,ProductId,Discount")] DiscountedProduct discountedProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountedProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", discountedProduct.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id", discountedProduct.SaleId);
            return View(discountedProduct);
        }

        // GET: DiscountedProducts/Edit/5
        public async Task<IActionResult> Edit(int? productId, int? saleId)
        {
            if (productId == null || saleId == null)
            {
                return NotFound();
            }

            var discountedProduct = await _context.DiscountedProduct.FindAsync(productId, saleId);
            if (discountedProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", discountedProduct.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id", discountedProduct.SaleId);
            return View(discountedProduct);
        }

        // POST: DiscountedProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleId,ProductId")] DiscountedProduct discountedProduct)
        {
            if (id != discountedProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountedProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountedProductExists(discountedProduct.ProductId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", discountedProduct.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id", discountedProduct.SaleId);
            return View(discountedProduct);
        }

        // GET: DiscountedProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountedProduct = await _context.DiscountedProduct
                .Include(d => d.Product)
                .Include(d => d.Sale)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (discountedProduct == null)
            {
                return NotFound();
            }

            return View(discountedProduct);
        }

        // POST: DiscountedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discountedProduct = await _context.DiscountedProduct.FindAsync(id);
            _context.DiscountedProduct.Remove(discountedProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountedProductExists(int id)
        {
            return _context.DiscountedProduct.Any(e => e.ProductId == id);
        }
        //TODO: dokonczyc ASAP- mam liste productID, teraz musze wypisac wszystkie produktu o danych productID
        //public async Task<IActionResult> ProductsOnThisSale(int id)
        //{
        //    var q = _context.DiscountedProduct.Where(c => c.SaleId.Equals(id));
        //    var b = _context.Products.Where(d => d.Id.Equals(q);
        //    return View(await b.ToListAsync());
        //}
    }
}
