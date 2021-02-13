using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRHomePage.Entities;
using MRHomePage.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Database
{
    public class SQLiteDbContext : DbContext, IDbContext
    {
        internal static NLog.Logger dbLog { get; private set; } = NLog.LogManager.GetLogger("DBErrors");

        DbContextOptions _options;
        public SQLiteDbContext()
        {
                //options=> UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        }
        public SQLiteDbContext(DbContextOptions options) : base(options) //            : base("myConnectionString")
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MRDatabase.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
                
            options.UseSqlite(connection);
        }

        public DbSet<PricesToTrackModel> PricesToTrack { get; set; }
        public DbSet<PriceTrackerHistoryEntity> PriceTrackerHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //fluent configuration
        }

        public Task<int> SaveChangesAsync()
        {
            try
            {
                return new TaskFactory<int>().StartNew(() => this.SaveChanges());
            }
            catch (Exception ex)
            {
                dbLog.Error(ex);
                throw;
            }
        }
    }
}
