using NUnit.Framework;
using System.Linq;

namespace MRHomePage.Tests
{
    public class PriceDashboard
    {
        MRHomePage.Database.SQLiteDbContext sqLiteDbContext;

        [SetUp]
        public void Setup()
        {
            sqLiteDbContext = new Database.SQLiteDbContext();
        }

        [Test]
        public void isPricesToTrackTableExist()
        {
            var emptyTable = sqLiteDbContext.PricesToTrack;
            Assert.NotNull(emptyTable);
        }

        [Test]
        public void isGetDataFromWebWorkCorrectly()
        {
            var wikiPedia = MRHomePage.Controllers.PriceDashboardController.GetDataFromWeb("https://en.wikipedia.org/wiki/Static_variable", "//*[@id=\"firstHeading\"]/text()").Result;
            Assert.AreEqual("Static variable", wikiPedia);
        }
    }
}