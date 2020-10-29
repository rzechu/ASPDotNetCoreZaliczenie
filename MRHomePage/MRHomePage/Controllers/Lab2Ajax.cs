using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MRHomePage.Models;

namespace MRHomePage.Controllers
{
    [ApiController]
    [Route("api/Lab2Ajax")]
    public class Lab2Ajax : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            //RedirectToAction("Post", new ItemModel() { Description = "Testowy Opis", Name = "Testowy Name", IsVisible = true });
            return View("Add");
        }

        [HttpPost]
        public ItemModel Post(ItemModel model)
        {
            return model;
        }
    }
}
