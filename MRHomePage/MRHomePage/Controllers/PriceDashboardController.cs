using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRHomePage.Database;
using MRHomePage.Helpers;
using MRHomePage.Interfaces;

namespace MRHomePage.Controllers
{
    [Authorize]
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

        public IActionResult Edit(int Id)
        {
            try
            {
                var itemToEdit = _dbContext.PricesToTrack.Where(w => w.Id == Id).FirstOrDefault();
                Models.PriceTracker priceTrackToEdit = new Models.PriceTracker()
                {
                    Id = itemToEdit.Id,
                    Name = itemToEdit.Name,
                    URL = itemToEdit.URL,
                    XPath = itemToEdit.XPath
                };
                var trackedPrices = GetPricesToTrack();
                ViewBag.PricesToTrack = trackedPrices;
                
                return View(nameof(Index), priceTrackToEdit);
            }
            catch(Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        public async Task<IActionResult> EditSave(Models.PriceTracker editedPrice)
        {
            try
            {
                var itemToEdit = _dbContext.PricesToTrack.Where(w => w.Id == editedPrice.Id).FirstOrDefault();
                itemToEdit.Name = editedPrice.Name;
                itemToEdit.URL = editedPrice.URL;
                itemToEdit.XPath = editedPrice.XPath;
                _dbContext.PricesToTrack.Update(itemToEdit);
                await _dbContext.SaveChangesAsync();

                editedPrice = null;

                var trackedPrices = GetPricesToTrack();
                ViewBag.PricesToTrack = trackedPrices;

                return RedirectToAction("Index", "PriceDashboard");
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var itemToRemove = _dbContext.PricesToTrack.Where(w => w.Id == Id).FirstOrDefault();
                _dbContext.PricesToTrack.Remove(itemToRemove);
                await _dbContext.SaveChangesAsync();
                var trackedPrices = GetPricesToTrack();
                ViewBag.PricesToTrack = trackedPrices;

                return View("Index");
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }

        }

        public async Task<IActionResult> GetData()
        {
            GetDataWeb();
            var trackedPrices = GetPricesToTrack();
            ViewBag.PricesToTrack = trackedPrices;

            return View("Index");
        }

        public async Task<IActionResult> AddPriceToTrack(Models.PriceTracker PriceTracker)
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
                await _dbContext.SaveChangesAsync();

                var trackedPrices = GetPricesToTrack();
                ViewBag.PricesToTrack = trackedPrices;

                return View(nameof(Index));
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        #region private methods
        private List<Models.PriceTracker> GetPricesToTrack()
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
                            LastValue = item.LastValue,
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

        private async void GetDataWeb()
        {
            try
            {
                var pricesToTrack = _dbContext.PricesToTrack;
                foreach (var price in pricesToTrack)
                {
                    price.LastValue = await GetDataFromWeb(price.URL, price.XPath);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        private async Task<string> GetDataFromWeb(string Url, string XPath)
        {
            try
            {
                List<string> result = new List<string>();

                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                var resultPage = web.Load(Url);
                var node = resultPage.DocumentNode.SelectNodes(XPath);
                if (node != null)
                {
                    foreach (var item in node)
                    {
                        if(!String.IsNullOrEmpty(item.InnerText))
                        {
                            result.Add(item.InnerText);

                            //if (item.InnerLength>100)
                            //    result.Add(item.InnerText.Trim().Substring(0, 100) );
                            //else
                            //    result.Add(item.InnerText.Trim().Substring(0, item.InnerText.Length-1));
                        }
                    }
                }
                return String.Join(System.Environment.NewLine, result);
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw new Exception($"Błąd podczas pobierania danych {System.Reflection.MethodBase.GetCurrentMethod().Name} z {Url}: {ex.Message}");
            }
        }
        #endregion

        #region old - to delete
        //public void OldGetDataWeb()
        //{
        //    try
        //    {
        //        List<string> xPathsToCheck = new List<string>();
        //        xPathsToCheck.Add("/html/body/div[2]/div[4]/div/div/div[2]/div[1]/div[1]/h1/text()");
        //        xPathsToCheck.Add("/html/body/div[2]/div[4]/div/div/div[2]/div[1]/div[2]/div/div/div[2]/div[2]/div[1]/div[1]/em");
        //        OldGetDataFromWeb(@"https://ripe.pl/pl/p/Terminal-HP-t620-QUAD-CORE-16GF4GR-Zasilacz/10629", xPathsToCheck);
        //    }
        //    catch (Exception ex)
        //    {
        //        WebAppException.LogException(ex);
        //        throw;
        //    }
        //}

        //public List<string> OldGetDataFromWeb(string Url, List<string> XPaths)
        //{
        //    List<string> downloadedNodes = new List<string>();
        //    try
        //    {
        //        HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
        //        var resultPage = web.Load(Url);
        //        foreach (var xPath in XPaths)
        //        {
        //            var node = resultPage.DocumentNode.SelectNodes(xPath);
        //            if (node != null)
        //            {
        //                foreach (var item in node)
        //                {
        //                    downloadedNodes.Add(item.InnerText.Trim());

        //                }
        //            }
        //        }
        //        return downloadedNodes;
        //    }
        //    catch (Exception ex)
        //    {
        //        WebAppException.LogException(ex);
        //        throw new Exception($"Błąd podczas pobierania danych {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
        //    }
        //}
        #endregion
    }
}