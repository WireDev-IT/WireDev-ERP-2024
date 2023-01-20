using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Purchase
    {
        public Purchase()
        {
            Uuid = Guid.NewGuid();
        }

        [Key]
        public Guid Uuid { get; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal TotalPrice { get; private set; } = 0;
        public DateTime? DatePosted { get; private set; } = null;
        public List<TransactionItem> Items { get; private set; } = new();
        public bool Posted { get; private set; } = false;

        public bool TryAddItem(uint productId, Guid priceId, TransactionType type, uint itemCount)
        {
            if (!Posted)
            {
                Items.Add(new TransactionItem(productId, priceId, type, itemCount));

                //TODO: Find price
                Price price = new();

                if (type == TransactionType.Sell)
                {
                    TotalPrice = decimal.Add(TotalPrice, price.SellValue);
                }
                else if (type == TransactionType.Disposed || type == TransactionType.Purchase)
                {
                    TotalPrice = decimal.Subtract(TotalPrice, price.RetailValue);
                }
                else
                {
                    TotalPrice = decimal.Subtract(TotalPrice, price.SellValue);
                }

                return true;
            }
            return false;
        }

        public void Post()
        {
            Posted = true;
            DatePosted = DateTime.UtcNow;
        }
    }
}