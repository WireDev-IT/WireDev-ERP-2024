using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using WireDev.Erp.V1.Models.AuditLog.Models;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.AuditLog.Trail
{
    public class AuditEntry
    {
        public EntityEntry Entry { get; }
        public AuditType AuditType { get; set; }
        public string AuditUser { get; }
        public string TableName { get; set; }
        public Dictionary<string, object?> KeyValues { get; } = new();
        public Dictionary<string, object?> OldValues { get; } = new();
        public Dictionary<string, object?> NewValues { get; } = new();
        public List<string> ChangedColumns { get; } = new List<string>();

        public AuditEntry(EntityEntry entry, string auditUser)
        {
            Entry = entry;
            AuditUser = auditUser;
            TableName = Entry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() is TableAttribute tableAttr ? tableAttr.Name : Entry.Entity.GetType().Name;
            SetChanges();
        }

        private void SetChanges()
        {
            foreach (PropertyEntry property in Entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                string? dbColumnName = property.Metadata.Name; //not name of column

                if (property.Metadata.IsPrimaryKey())
                {
                    KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (Entry.State)
                {
                    case EntityState.Added:
                        NewValues[propertyName] = property.CurrentValue;
                        AuditType = AuditType.Create;
                        break;

                    case EntityState.Deleted:
                        OldValues[propertyName] = property.OriginalValue;
                        AuditType = AuditType.Delete;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            ChangedColumns.Add(dbColumnName);

                            OldValues[propertyName] = property.OriginalValue;
                            NewValues[propertyName] = property.CurrentValue;
                            AuditType = AuditType.Update;
                        }
                        break;
                }
            }
        }

        public Audit ToAudit()
        {
            Audit audit = new()
            {
                Id = Guid.NewGuid(),
                AuditDateTimeUtc = DateTime.UtcNow,
                AuditType = AuditType.ToString(),
                AuditUser = AuditUser,
                TableName = TableName,
                KeyValues = JsonSerializer.Serialize(KeyValues, new JsonSerializerOptions() { WriteIndented = true }),
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues, new JsonSerializerOptions() { WriteIndented = true }),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues, new JsonSerializerOptions() { WriteIndented = true }),
                ChangedColumns = ChangedColumns.Count == 0 ? null : JsonSerializer.Serialize(ChangedColumns, new JsonSerializerOptions() { WriteIndented = true })
            };

            return audit;
        }
    }
}
