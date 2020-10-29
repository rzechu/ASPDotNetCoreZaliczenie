using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Helpers
{
    public interface IControllers
    {
        public abstract string ControllerName { get; set; }
    }
}
