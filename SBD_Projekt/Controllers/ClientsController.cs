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
        public ActionResult Register(Client _client)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Clients.FirstOrDefault(s => s.Email == _client.Email);
                if (check == null)
                {
                    _client.Password = GetMD5(_client.Password);
                    _context.Clients.Add(_client);
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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = _context.Clients.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    HttpContext.Session.SetString("Username", data.FirstOrDefault().Username);
                    HttpContext.Session.SetString("Email", data.FirstOrDefault().Email);
                    HttpContext.Session.SetInt32("Id", data.FirstOrDefault().Id);

                    return RedirectToAction("Index", "Home"); //TODO: przekierowac do strony ktora wywolala login
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Login");
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
