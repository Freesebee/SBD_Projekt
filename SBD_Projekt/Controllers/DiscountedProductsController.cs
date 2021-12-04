﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDProjekt.Infrastructure;
using SBDProjekt.Models;

namespace SBD_Projekt.Controllers
{
    public class DiscountedProductsController : Controller
    {
        private readonly MyDBContext _context;

        public DiscountedProductsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: DiscountedProducts
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.DiscountedProduct.Include(d => d.Product).Include(d => d.Sale);
            return View(await myDBContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["SaleId"] = new SelectList(_context.Sales, "Id", "Id");
            return View();
        }

        // POST: DiscountedProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,ProductId")] DiscountedProduct discountedProduct)
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountedProduct = await _context.DiscountedProduct.FindAsync(id);
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
    }
}