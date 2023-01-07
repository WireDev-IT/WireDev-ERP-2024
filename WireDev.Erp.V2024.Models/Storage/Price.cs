using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WireDev.Erp.V1.Models.Interfaces;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Price : IPrice
    {
        public Price()
        {
            Uuid = Guid.NewGuid();
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
    }
}