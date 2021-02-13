using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Models
{
    public class PriceTracker
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Url]
        public string URL { get; set; }
        [Required]
        public string XPath { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
