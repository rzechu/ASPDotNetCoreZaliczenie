using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Models
{
    public class OpenWeatherMap
    {
        public bool isApiKeyDefault { get; set; }
        public string apiResponse { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double WindSpeed { get; set; }
        public double Temp { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> cities
        {
            get; set;
        }
    }
}
