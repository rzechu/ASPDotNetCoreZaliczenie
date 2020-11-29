using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Models
{
    public class PriceTrackerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string XPath { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
