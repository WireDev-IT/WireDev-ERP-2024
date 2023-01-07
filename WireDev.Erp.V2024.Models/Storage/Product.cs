using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using WireDev.Erp.V1.Models.Statistics;
using System.Collections.Generic;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Product
    {
        public Product()
        {
            DateCreated = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
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
        public List<Guid> Categories { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}