using System;
using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Price : IPrice
    {
        public Price(decimal retailValue, decimal sellValue, string? description = null)
        {
            RetailValue = retailValue;
            SellValue = sellValue;
            Description = description;
            Uuid = Guid.NewGuid();
        }

        [Key]
        public Guid Uuid { get; }
        public bool Archived { get; set; } = false;

        private string? _description;
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

        private decimal _retailValue;
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

        private decimal _sellValue;
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
    }
}