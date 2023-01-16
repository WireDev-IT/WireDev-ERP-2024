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
    [PrimaryKey("Uuid")]
    public class Product
    {
        public Product()
        {

        }

        public Product(uint articelNumber)
        {
            DateCreated = DateTime.UtcNow;
            Uuid = articelNumber;
        }

        public DateTime DateCreated { get; }

        public bool Used { get; private set; } = false;
        public void Use() => Used = true;

        [Key]
        public uint Uuid { get; }
        public int Group { get; set; } = 100;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product needs a name.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; } = false;
        public bool Archived { get; set; } = false;
        public uint Availible { get; private set; } = 0;

        public List<ulong> EAN { get; set; } = new();
        public List<Price> Prices { get; set; } = new();
        public List<Guid> Categories { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();

        public uint Add(uint add) => Availible += add;
        public uint Remove(uint add) => Availible -= add;
    }
}