using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
        public void Use()
        {
            Used = true;
        }

        [Key]
        public uint Uuid { get; }
        public int Group { get; set; } = 100;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product needs a name.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; } = false;
        public bool Archived { get; set; } = false;
        public int Availible { get; private set; } = 0;

        public List<ulong> EAN { get; set; } = new();
        public List<Guid> Prices { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
        public Dictionary<string, string> Metadata { get; set; } = new();

        public int Add(uint add)
        {
            return Availible += (int)add;
        }

        public int Remove(uint add)
        {
            return Availible -= (int)add;
        }

        public Task ModifyProperties(Product product)
        {
            string[] propertyNames = { "Uuid" };
            PropertyInfo[] properties = typeof(Product).GetProperties();
            foreach (PropertyInfo sPI in properties)
            {
                if (!propertyNames.Contains(sPI.Name))
                {
                    PropertyInfo? tPI = GetType().GetProperty(sPI.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (tPI != null && tPI.CanWrite && tPI.PropertyType.IsAssignableFrom(sPI.PropertyType))
                    {
                        tPI.SetValue(this, sPI.GetValue(product, null), null);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}