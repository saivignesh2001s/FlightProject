using Flights.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Flights.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login p)
        {
            if(p.username.ToString()=="admin@gmail.com" && p.password.ToString()=="Admin@123")
            {
                HttpContext.Session.SetString("currentUser",p.username);
                return RedirectToAction("Flightlist","Flight");
            }
            else if(p.username.ToString() == "admin@gmail.com" && p.password.ToString() != "Admin@123")
            {
                ViewBag.Message1 = "Check the password";
                return View();
            }
            else if(p.username.ToString() != "admin@gmail.com" && p.password.ToString() == "Admin@123")
            {

                ViewBag.Message2 = "Check the username";
                return View();

            }
            else
            {
                ViewBag.Message3 = "Enter the correct email id and password";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}