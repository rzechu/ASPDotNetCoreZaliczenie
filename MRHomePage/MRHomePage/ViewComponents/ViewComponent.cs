using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRHomePage.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.ViewComponents
{
    public class LatestPrice : ViewComponent
    {
        private readonly MRDbContext dbContext = null;
        public LatestPrice(MRDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public IViewComponentResult Invoke()
        {
            var latestPrice = dbContext.PricesToTrack.OrderByDescending(x => x.Id).First();
            return View("Index", latestPrice);
        }
    }
}
