using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WireDev.Erp.V1.Models.AuditLog.Contract;
using WireDev.Erp.V1.Models.AuditLog.Models;

namespace WireDev.Erp.V1.Models.AuditLog.Trail
{
    public class AuditHelper
    {
        private readonly IAuditDbContext Db;

        public AuditHelper(IAuditDbContext db)
        {
            Db = db;
        }

        public void AddAuditLogs(string? userName)
        {
            Db.ChangeTracker.DetectChanges();
            List<AuditEntry> auditEntries = new();
            foreach (EntityEntry entry in Db.ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }
                AuditEntry auditEntry = new(entry, userName);
                auditEntries.Add(auditEntry);
            }

            if (auditEntries.Any())
            {
                IEnumerable<Audit> logs = (IEnumerable<Audit>)auditEntries.Select(x => x.ToAudit());
                Db.Audit.AddRange(logs);
            }
        }
    }
}
