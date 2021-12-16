namespace SBD_Projekt.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SBDProjekt.Infrastructure;
    using Microsoft.AspNetCore.Http;
    using System.Text;
    using System.Security.Cryptography;
    using SBDProjekt.Models;
    using Microsoft.AspNetCore.Authorization;
    using SBD_Projekt.Models;
    using Microsoft.AspNetCore.Authentication;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;

    public class ClientsController : Controller
    {
        private readonly MyDBContext _context;

        public ClientsController(MyDBContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Id") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Clients.FirstOrDefault(s => s.Email == user.Email);

                var client = new Client();

                if (check == null)
                {
                    user.Password = GetMD5(user.Password);

                    client.Id = user.Id;
                    client.Password = user.Password;
                    client.Email = user.Email;
                    client.Username = user.UserName;

                    _context.Users.Add(user);
                    _context.Clients.Add(client);

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var appUser = _context.Users.FirstOrDefault(
                    s => s.Email.Equals(email) 
                    && s.Password.Equals(f_password));

                if (appUser != null)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Email, email));

                    var ur = _context.UserRoles.FirstOrDefault(ur => ur.UserId == appUser.Id);
                    var r = _context.Roles.FirstOrDefault(r => r.NormalizedName == "ADMIN");

                    if(ur != null && r != null && ur.RoleId == r.Id)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );
                    var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrinciple);

                    return returnUrl != null ? Redirect(returnUrl) : Redirect("Index");
                }
                else
                {
                    TempData["Error"] = "User email or password is not valid";
                    return RedirectToAction("Login", new { ReturnUrl = returnUrl});
                }
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}
