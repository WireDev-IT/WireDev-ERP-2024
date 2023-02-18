using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WireDev.Erp.V1.Models.AuditLog.Models;

namespace WireDev.Erp.V1.Models.AuditLog.Contract
{
    public interface IAuditDbContext
    {
        DbSet<Audit> Audit { get; set; }
        ChangeTracker ChangeTracker { get; }
    }
}