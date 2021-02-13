using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MRHomePage.Helpers;
using MRHomePage.Models;

namespace MRHomePage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel Login, string ReturnUrl)
        {
            Login.RedirectUrl = ReturnUrl;
            if(String.Equals(Login.UserName,"MR", StringComparison.InvariantCultureIgnoreCase) && String.Equals(Login.UserPassword, "123"))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Login.UserName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(Login.RedirectUrl == null ? "/Home" : Login.RedirectUrl);
            }
            else
            {
                WebAppException.webLog.Trace($"Wrong credentials entered Login={Login.UserName ?? ""} Password={Login.UserPassword ?? ""}");

                Login.IsWrongCredentials = true;
                Login.UserPassword = string.Empty;
                return View(Login);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
