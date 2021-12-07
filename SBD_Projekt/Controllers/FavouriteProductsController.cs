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

namespace SBD_Projekt.Controllers
{
    public class FavouriteProductsController : Controller
    {
        private readonly MyDBContext _context;

        public FavouriteProductsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: FavouriteProducts
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.FavouriteProduct.Include(f => f.Client).Include(f => f.Product);
            return View(await myDBContext.ToListAsync());
        }

        // GET: FavouriteProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteProduct = await _context.FavouriteProduct
                .Include(f => f.Client)
                .Include(f => f.Product)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (favouriteProduct == null)
            {
                return NotFound();
            }

            return View(favouriteProduct);
        }

        // GET: FavouriteProducts/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: FavouriteProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ClientId")] FavouriteProduct favouriteProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favouriteProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", favouriteProduct.ClientId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", favouriteProduct.ProductId);
            return View(favouriteProduct);
        }

        // GET: FavouriteProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteProduct = await _context.FavouriteProduct.FindAsync(id);
            if (favouriteProduct == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", favouriteProduct.ClientId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", favouriteProduct.ProductId);
            return View(favouriteProduct);
        }

        // POST: FavouriteProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ClientId")] FavouriteProduct favouriteProduct)
        {
            if (id != favouriteProduct.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favouriteProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouriteProductExists(favouriteProduct.ClientId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", favouriteProduct.ClientId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", favouriteProduct.ProductId);
            return View(favouriteProduct);
        }

        // GET: FavouriteProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favouriteProduct = await _context.FavouriteProduct
                .Include(f => f.Client)
                .Include(f => f.Product)
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (favouriteProduct == null)
            {
                return NotFound();
            }

            return View(favouriteProduct);
        }

        // POST: FavouriteProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favouriteProduct = await _context.FavouriteProduct.FindAsync(id);
            _context.FavouriteProduct.Remove(favouriteProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavouriteProductExists(int id)
        {
            return _context.FavouriteProduct.Any(e => e.ClientId == id);
        }
    }
}
