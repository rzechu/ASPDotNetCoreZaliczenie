using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Entities
{
    public class PricesToTrackModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string XPath { get; set; }
        public string LastValue { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
