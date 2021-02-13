using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MRHomePage.Database;
using MRHomePage.Helpers;
using MRHomePage.Interfaces;

namespace MRHomePage.Controllers
{
    public class PriceDashboardController : Controller
    {
        //private readonly IDbContext _dbContext;
        private readonly SQLiteDbContext _dbContext = new SQLiteDbContext();
        public PriceDashboardController(/*SQLiteDbContext dbContext*/)
        {
            //_dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var trackedPrices = GetPricesToTrack();
            ViewBag.PricesToTrack = trackedPrices;

            return View();
        }

        public void AddPriceToTrack(Models.PriceTracker PriceTracker)
        {
            try
            {
                Entities.PricesToTrackModel priceDb = new Entities.PricesToTrackModel()
                {
                    Name = PriceTracker.Name,
                    URL = PriceTracker.URL,
                    XPath = PriceTracker.XPath,
                    UpdatedDate = DateTime.Now
                };

                _dbContext.PricesToTrack.Add(priceDb);
                _dbContext.SaveChangesAsync();

                var trackedPrices = GetPricesToTrack();
            }
            catch(Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        public List<Models.PriceTracker> GetPricesToTrack()
        {
            try
            {
                if (_dbContext != null && _dbContext.PricesToTrack != null && _dbContext.PricesToTrack.Count() > 0)
                {
                    List<Models.PriceTracker> prices = new List<Models.PriceTracker>();
                    foreach (var item in _dbContext.PricesToTrack)
                    {
                        prices.Add(new Models.PriceTracker()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            URL = item.URL,
                            XPath = item.XPath,
                            UpdatedDate = item.UpdatedDate
                        });
                    }
                    return prices;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        public void GetDataWeb()
        {
            try
            {
                List<string> xPathsToCheck = new List<string>();
                xPathsToCheck.Add("/html/body/div[2]/div[4]/div/div/div[2]/div[1]/div[1]/h1/text()");
                xPathsToCheck.Add("/html/body/div[2]/div[4]/div/div/div[2]/div[1]/div[2]/div/div/div[2]/div[2]/div[1]/div[1]/em");
                GetDataFromWeb(@"https://ripe.pl/pl/p/Terminal-HP-t620-QUAD-CORE-16GF4GR-Zasilacz/10629", xPathsToCheck);
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
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
                WebAppException.LogException(ex);
                throw new Exception($"Błąd podczas pobierania danych {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }
    }
}
