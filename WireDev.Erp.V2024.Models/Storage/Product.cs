using System.ComponentModel.DataAnnotations;
using WireDev.Erp.V1.Models.Statistics;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Product
    {
        public Product(string name, string? description = null, bool active = false)
        {
            DateCreated = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
            Name = name;
            Active = active;
        }

        public DateTime DateCreated { get; }

        public bool Used { get; private set; } = false;
        public void Use() => Used = true;

        [Key]
        public Guid Uuid { get; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public bool Archived { get; set; } = false;

        public List<Guid> Prices { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}