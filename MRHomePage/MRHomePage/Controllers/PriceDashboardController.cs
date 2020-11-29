using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MRHomePage.Database;

namespace MRHomePage.Controllers
{
    public class PriceDashboardController : Controller
    {
        private readonly MRDbContext _dbContext;
        public PriceDashboardController(MRDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            GetPricesToTrack();


            return View();
        }

        public void AddPriceToTrack(Entities.PricesToTrackModel PriceToTrack)
        {
            Entities.PricesToTrackModel priceDb = new Entities.PricesToTrackModel()
            {
                Name = PriceToTrack.Name,
                URL = PriceToTrack.URL,
                XPath = PriceToTrack.XPath
            };

            _dbContext.PricesToTrack.Add(priceDb);
            _dbContext.SaveChanges();

            GetPricesToTrack();
        }

        public void GetPricesToTrack()
        {
            ViewBag.PricesToTrack = _dbContext.PricesToTrack;
        }

        public void GetDataWeb()
        {
            List<string> xPathsToCheck = new List<string>();
            xPathsToCheck.Add("/html/body/div[2]/div[4]/div/div/div[2]/div[1]/div[1]/h1/text()");
            xPathsToCheck.Add("/html/body/div[2]/div[4]/div/div/div[2]/div[1]/div[2]/div/div/div[2]/div[2]/div[1]/div[1]/em");
            GetDataFromWeb(@"https://ripe.pl/pl/p/Terminal-HP-t620-QUAD-CORE-16GF4GR-Zasilacz/10629", xPathsToCheck);
        }

        public List<string> GetDataFromWeb(string Url, List<string> XPaths)
        {
            List<string> downloadedNodes = new List<string>();
            try
            {
                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                var resultPage = web.Load(Url);
                foreach (var xPath in XPaths)
                {
                    var node = resultPage.DocumentNode.SelectNodes(xPath);
                    if (node != null)
                    {
                        foreach (var item in node)
                        {
                            downloadedNodes.Add(item.InnerText.Trim());

                        }
                    }
                }
                return downloadedNodes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Błąd podczas pobierania danych {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }
    }
}
