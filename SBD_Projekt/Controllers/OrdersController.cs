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

namespace SBDProjekt.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly MyDBContext _context;

        public OrdersController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Order> orders = await _context.Orders.Where(o => o.ClientId == userId).ToListAsync();

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            string cartJSON = HttpContext.Session.GetString(SessionConsts.CartKey);
            
            if (cartJSON == null) return View("Index");

            Order newOrder = new Order()
            {
                ClientId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                OrderedProduct = JsonConvert.DeserializeObject<List<OrderedProduct>>(cartJSON)
            };

            foreach (var item in newOrder.OrderedProduct)
            {
                item.Product = null;
            }

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = newOrder.Id });
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderedProduct)
                .ThenInclude(op => op.Product)
                .First(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }
    }
}
