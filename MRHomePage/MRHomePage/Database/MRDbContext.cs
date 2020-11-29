using Microsoft.EntityFrameworkCore;
using MRHomePage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Database
{
    public class MRDbContext : DbContext
    {
        public MRDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PricesToTrackModel> PricesToTrack { get; set; }
        public DbSet<PriceTrackerHistoryEntity> PriceTrackerHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //fluent configuration
        }
    }
}
