using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models.Storage
{
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
        public int Availible { get; private set; } = 0;

        public ObservableCollection<ulong> EAN { get; set; } = new();
        public List<Guid> Prices { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();

        public int Add(uint add) => Availible += (int)add;
        public int Remove(uint add) => Availible -= (int)add;
    }
}