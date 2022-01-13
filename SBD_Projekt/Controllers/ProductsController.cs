using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SBD_Projekt.Consts;
using SBDProjekt.Infrastructure;
using SBDProjekt.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SBD_Projekt.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyDBContext _context;

        public ProductsController(MyDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .ToListAsync()
            );
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            DetailsProductViewModel model = new DetailsProductViewModel();
            model.CategoryName = _context
                .Categories
                .Single(p => p.Id == product.CategoryId).Name;

            model.ManufacturerName = _context
                .Manufacturers
                .Single(p => p.Id == product.ManufacturerId).Name;
            model.Id = product.Id;
            model.ManufacturerId = product.ManufacturerId;
            model.Name = product.Name;
            model.Price = product.Price;
            model.OpinionList = _context.Opinions.Where(p => p.ProductId == product.Id).ToList();

            return View(model);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync()
        {
            EditProductViewModel model = new EditProductViewModel();
            model.CategoryList = await _context.Categories.ToListAsync();
            model.ManufacturerList = await _context.Manufacturers.ToListAsync();

            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Price,Name,Details,CategoryId,ManufacturerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            EditProductViewModel model = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryList = await _context.Categories.ToListAsync(),
                ManufacturerId = product.ManufacturerId,
                ManufacturerList = await _context.Manufacturers.ToListAsync(),
                Details = product.Details
            };

            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Name,Details,CategoryId,ManufacturerId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> AddToFavourites(int productId)
        {
            FavouriteProduct favouriteProduct = new FavouriteProduct();
            if (ModelState.IsValid && !FavouriteProductExists(productId, User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                favouriteProduct.ProductId = productId;
                //var usernameFromContext = HttpContext.User.Identity.Name;
                //Client client = _context.Clients.Single(p => p.Username == usernameFromContext);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                favouriteProduct.ClientId = userId;
                _context.Add(favouriteProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = productId });
            }

            return RedirectToAction("Details", new { id = productId });
        }

        private bool FavouriteProductExists(int productId, string clientId)
        {
            return _context.FavouriteProduct.Any(e => e.ProductId == productId && e.ClientId == clientId);
        }

        public IActionResult Cart()
        {
            return View(_GetCartProducts());
        }

        public JsonResult AddToCart(int productId)
        {
            if (_context.Products.Find(productId) == null) return Json(NotFound("Product with this ID was not found"));

            List<OrderedProduct> cart;

            if (HttpContext.Session.GetString(SessionConsts.CartKey) == null)
            {
                cart = new List<OrderedProduct>();
            }
            else
            {
                cart = _GetCartProducts();
            }

            OrderedProduct existingOrder = null;

            if (cart.Count > 0)
            {
                existingOrder = cart.Find(op => op.ProductId == productId);
            }

            if (existingOrder != null)
            {
                existingOrder.Quantity++;
            }
            else
            {
                var order = new OrderedProduct();
                order.Product = _context.Products.Find(productId);
                order.ProductId = order.Product.Id;
                order.Quantity = 1;
                cart.Add(order);
            }

            HttpContext.Session.SetString(SessionConsts.CartKey, JsonConvert.SerializeObject(cart));

            return Json(cart);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            if (_context.Products.Find(productId) == null) return NotFound("Product with this ID was not found");

            List<OrderedProduct> cart;

            if (HttpContext.Session.GetString(SessionConsts.CartKey) == null)
            {
                cart = new List<OrderedProduct>();
            }
            else
            {
                cart = _GetCartProducts();
            }

            OrderedProduct existingOrder = null;

            if (cart.Count > 0)
            {
                existingOrder = cart.Find(op => op.ProductId == productId);
            }

            if (existingOrder != null)
            {
                if (existingOrder.Quantity > 1)
                {
                    existingOrder.Quantity--;
                }
                else
                {
                    cart.RemoveAll(p => p.ProductId == productId);
                }

                HttpContext.Session.SetString(SessionConsts.CartKey, JsonConvert.SerializeObject(cart));
            }

            return View("Cart", cart);
        }

        public List<OrderedProduct> _GetCartProducts()
        {
            var cart = HttpContext.Session.GetString(SessionConsts.CartKey);

            return cart != null ? JsonConvert.DeserializeObject<List<OrderedProduct>>(cart) : null;
        }
    }
}
