using System;
using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Models.Statistics;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
	public class StatsDbContext : DbContext
	{
		public StatsDbContext(DbContextOptions<StatsDbContext> options) : base(options)
        {

		}

        public virtual DbSet<TotalStats> TotalStats { get; set; }
        public virtual DbSet<YearStats> YearStats { get; set; }
        public virtual DbSet<MonthStats> MonthStats { get; set; }
        public virtual DbSet<DayStats> DayStats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            _ = builder.Entity<DayStats>().HasKey("Date");
            _ = builder.Entity<MonthStats>().HasKey("Date");
            _ = builder.Entity<YearStats>().HasKey("Date");
            _ = builder.Entity<TotalStats>().HasKey("Date");
        }
    }
}