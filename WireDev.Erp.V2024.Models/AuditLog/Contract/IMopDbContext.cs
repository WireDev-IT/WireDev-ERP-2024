using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WireDev.Erp.V1.Models.AuditLog.Models;

namespace WireDev.Erp.V1.Models.AuditLog.Contract
{
    public interface IMopDbContext : IAuditDbContext, IDisposable
    {
        DbSet<Role> Role { get; set; }

        DatabaseFacade Database { get; }
        int SaveChanges(string userName);
    }
}
