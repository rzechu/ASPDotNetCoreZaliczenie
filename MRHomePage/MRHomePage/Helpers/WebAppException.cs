using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Helpers
{
    public class WebAppException : Exception
    {
        public WebAppException(IControllers controller)
        {
            ControllerName = controller;
        }

        public IControllers ControllerName { get; set; }
        internal static NLog.Logger webLog { get; private set; } = NLog.LogManager.GetLogger("WebErrors");

        public static void LogException(Exception ex, string AdditionalInfo = null)
        {
            if(String.IsNullOrEmpty(AdditionalInfo))
            {
                webLog.Error(ex);
            }
            else
            {
                webLog.Error(ex, AdditionalInfo);
            }
        }
    }
}
