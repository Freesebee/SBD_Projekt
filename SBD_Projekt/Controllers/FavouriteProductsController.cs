using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBDProjekt.Infrastructure;

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
            var myDBContext = _context.FavouriteProduct.Include(f => f.Client).Include(f => f.Product)
                .Where(g => g.ClientId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(await myDBContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {

            if (productId == 0)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favouriteProduct = await _context.FavouriteProduct.FindAsync(userId, productId);
            _context.FavouriteProduct.Remove(favouriteProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
