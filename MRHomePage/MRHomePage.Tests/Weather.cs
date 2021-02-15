using NUnit.Framework;
using System.Linq;

namespace MRHomePage.Tests
{
    public class Weather
    {
        static string defaultAPIKey = "getYourOwnAPIKey";
        static string otherAPIKey = "386d68fYourAPIKEY3895395f";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void isPricesToTrackTableExist()
        {
            var cityLists = MRHomePage.Controllers.WeatherController.FillCity(defaultAPIKey);
            Assert.AreEqual(6, cityLists.cities.Count);
        }

        [TestCase(true, "getYourOwnAPIKey")]
        [TestCase(false,"otherAPIKey")]
        public void isDefaultAPIKey(bool expectedResult, string apiKey)
        {
            var result = MRHomePage.Controllers.WeatherController.isDefaultAPIKey(apiKey);
            Assert.AreEqual(expectedResult, result);
        }
    }
}