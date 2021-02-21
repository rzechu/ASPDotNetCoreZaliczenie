using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRHomePage.Database;
using MRHomePage.Helpers;
using MRHomePage.Interfaces;

namespace MRHomePage.Controllers
{
    [Authorize]
    public class PriceDashboardController : Controller
    {
        //private readonly IDbContext _dbContext;
        private readonly static SQLiteDbContext _dbContext = new SQLiteDbContext();
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

        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var itemToEdit = await _dbContext.PricesToTrack.Where(w => w.Id == Id).FirstOrDefaultAsync();
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

        public IActionResult GetData()
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

                await _dbContext.PricesToTrack.AddAsync(priceDb);
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
                if (_dbContext != null && _dbContext.PricesToTrack != null && _dbContext.PricesToTrack.CountAsync().Result > 0)
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

        public static async void GetDataWeb()
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
                //throw;
            }
        }

        public static async Task<string> GetDataFromWeb(string Url, string XPath)
        {
            try
            {
                List<string> result = new List<string>();

                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                var resultPage = await web.LoadFromWebAsync(Url);
                var node = resultPage.DocumentNode.SelectNodes(XPath);
                if (node != null)
                {
                    foreach (var item in node)
                    {
                        if(!String.IsNullOrEmpty(item.InnerText))
                        {
                            result.Add(item.InnerText);
                        }
                    }
                }
                return String.Join(System.Environment.NewLine, result);
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex, $"Błąd podczas pobierania danych {System.Reflection.MethodBase.GetCurrentMethod().Name} z {Url}: {ex.Message}");
                //throw new Exception($"Błąd podczas pobierania danych {System.Reflection.MethodBase.GetCurrentMethod().Name} z {Url}: {ex.Message}");
                return  ex.Message;
            }
        }
        #endregion
    }
}