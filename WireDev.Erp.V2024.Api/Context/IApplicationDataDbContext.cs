using Microsoft.EntityFrameworkCore;
using WireDev.Erp.V1.Models;
using WireDev.Erp.V1.Models.AuditLog.Models;
using WireDev.Erp.V1.Models.Statistics;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Api.Context
{
    public interface IApplicationDataDbContext
    {
        DbSet<Audit> Audit { get; set; }
        DbSet<DayStats> DayStats { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<MonthStats> MonthStats { get; set; }
        DbSet<Price> Prices { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductStats> ProductStats { get; set; }
        DbSet<Purchase> Purchases { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<Settings> Settings { get; set; }
        DbSet<TotalStats> TotalStats { get; set; }
        DbSet<YearStats> YearStats { get; set; }

        int SaveChanges(string userName);
    }
}