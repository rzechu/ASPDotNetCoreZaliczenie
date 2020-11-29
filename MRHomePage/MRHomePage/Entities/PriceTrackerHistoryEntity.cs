using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Entities
{
    public class PriceTrackerHistoryEntity
    {
        public int Id { get; set; }
        public int PriceTrackerId { get; set; }
        public bool IsSuccess { get; set; }
        public string Price { get; set; }
        public DateTime CheckDate { get; set; }
    }
}
