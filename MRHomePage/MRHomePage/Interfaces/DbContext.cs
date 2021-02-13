using Microsoft.EntityFrameworkCore;
using MRHomePage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();

        public DbSet<PricesToTrackModel> PricesToTrack { get; set; }
        public DbSet<PriceTrackerHistoryEntity> PriceTrackerHistory { get; set; }
    }
}
