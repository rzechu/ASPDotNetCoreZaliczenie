﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using MRHomePage.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.IO;
using static MRHomePage.Models.OpenWeatherMapHelperClasses;
using System.Text;
using MRHomePage.Helpers;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace MRHomePage.Controllers
{
    [Authorize]
    public class WeatherController : Controller, Helpers.IControllers
    {
        public static string ControllerName = typeof(WeatherController).Name;
        string IControllers.ControllerName { get => ControllerName; set => ControllerName = value; }
        private static readonly  NLog.Logger logWeather  =  NLog.LogManager.GetCurrentClassLogger();

        private readonly IConfiguration _configuration;
        private readonly string APIKEY = string.Empty;
        public WeatherController(IConfiguration configuration)
        {
            _configuration = configuration;
            try
            {
                APIKEY = _configuration["OpenWeatherAPI"];
            }
            catch(WebAppException ex)
            {
                Helpers.WebAppException.LogException(ex);
                throw;
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                logWeather.Trace("WeatherController used");
                OpenWeatherMap openWeatherMap = FillCity(APIKEY);
                return View(openWeatherMap);
            }
            catch (WebAppException ex)
            {
                Helpers.WebAppException.LogException(ex);
                throw;
            }
        }

        public static bool isDefaultAPIKey(string APIKEY)
        {
            if (String.Equals(APIKEY, "getYourOwnAPIKey"))
                return true;
            else
                return false;
        }

        [HttpPost]
        public ActionResult Index(string cities)
        {
            try
            {
                OpenWeatherMap openWeatherMap = FillCity(APIKEY);

                if (cities != null)
                {
                    string apiKey = APIKEY;
                    HttpWebRequest apiRequest = WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=" + cities + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

                    string apiResponse = "";
                    using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        apiResponse = reader.ReadToEnd();
                    }

                    ResponseWeather rootObject = JsonSerializer.Deserialize<ResponseWeather>(apiResponse);
                    logWeather.Trace($"WeatherController used. Selected City = {rootObject.name}");
                    openWeatherMap.apiResponse = "Success";
                    openWeatherMap.City = rootObject.name ?? "";
                    openWeatherMap.Country = rootObject.sys.country ?? "";
                    openWeatherMap.WindSpeed = rootObject.wind.speed;
                    openWeatherMap.Temp = rootObject.main.temp;
                    openWeatherMap.Humidity = rootObject.main.humidity;
                    openWeatherMap.Description = rootObject.weather[0].description ??"";
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request.Form["submit"]))
                    {
                        openWeatherMap.apiResponse = "► Select City";
                    }
                }
                return View(openWeatherMap);
            }
            catch(WebAppException ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                WebAppException.LogException(ex);
                throw;
            }
        }

        public static OpenWeatherMap FillCity(string APIKEY)
        {
            OpenWeatherMap openWeatherMap = new OpenWeatherMap();
            openWeatherMap.cities = new Dictionary<string, string>();
            openWeatherMap.cities.Add("Kraków", "3094802");
            openWeatherMap.cities.Add("Warszawa", "6695624");
            openWeatherMap.cities.Add("Gdańsk", "3099434");
            openWeatherMap.cities.Add("Poznań", "3088171");
            openWeatherMap.cities.Add("Rzym", "3169070");
            openWeatherMap.cities.Add("Tel Aviv", "293397");

            openWeatherMap.isApiKeyDefault = isDefaultAPIKey(APIKEY);
            return openWeatherMap;
        }
    }
}