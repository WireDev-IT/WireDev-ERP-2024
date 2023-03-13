using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using WireDev.Erp.V1.Models.Interfaces;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Price : IPrice
    {
        public Price()
        {

        }

        public Price(Guid guid)
        {
            Uuid = guid;
        }

        [Key]
        public Guid Uuid { get; }
        public bool Archived { get; set; } = false;

        private string? _description = null;
        public string? Description
        {
            get => _description;
            set
            {
                if (!Locked)
                {
                    _description = value;
                }
            }
        }

        private decimal _retailValue = 0;
        [Column(TypeName = "decimal(5, 3)")]
        public decimal RetailValue
        {
            get => _retailValue;
            set
            {
                if (!Locked)
                {
                    _retailValue = value;
                }
            }
        }

        private decimal _sellValue = 0;
        [Column(TypeName = "decimal(5, 2)")]
        public decimal SellValue
        {
            get => _sellValue;
            set
            {
                if (!Locked)
                {
                    _sellValue = value;
                }
            }
        }

        public bool Locked { get; private set; } = false;
        public void Lock()
        {
            Locked = true;
        }

        public Task ModifyProperties(Price price)
        {
            string[] propertyNames = { "Uuid" };
            PropertyInfo[] properties = typeof(Price).GetProperties();
            foreach (PropertyInfo sPI in properties)
            {
                if (!propertyNames.Contains(sPI.Name))
                {
                    PropertyInfo? tPI = GetType().GetProperty(sPI.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (tPI != null && tPI.CanWrite && tPI.PropertyType.IsAssignableFrom(sPI.PropertyType))
                    {
                        tPI.SetValue(this, sPI.GetValue(price, null), null);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}