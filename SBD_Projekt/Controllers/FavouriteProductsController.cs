using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SBDProjekt.Infrastructure;
using SBDProjekt.Models;

namespace SBD_Projekt.Controllers
{
    [Authorize]
    public class FavouriteProductsController : Controller
    {
        private readonly MyDBContext _context;

        public FavouriteProductsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: FavouriteProducts
        public async Task<IActionResult> Index()
        {
            var myDBContext = _context.FavouriteProduct.Include(f => f.Client).Include(f => f.Product);
            return View(await myDBContext.ToListAsync());
        }

        // GET: FavouriteProducts/Details/5
        public async Task<IActionResult> Details(string id)
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
            
            return View();
        }

        // POST: FavouriteProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId)
        {
            FavouriteProduct favouriteProduct = new FavouriteProduct();
            if (ModelState.IsValid)
            {
                favouriteProduct.ProductId = productId;
                //var usernameFromContext = HttpContext.User.Identity.Name;
                //Client client = _context.Clients.Single(p => p.Username == usernameFromContext);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                favouriteProduct.ClientId = userId;
                _context.Add(favouriteProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        public async Task<IActionResult> Delete(string id)
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

        private bool FavouriteProductExists(string id)
        {
            return _context.FavouriteProduct.Any(e => e.ClientId == id);
        }
    }
}
