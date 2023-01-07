using System;
using System.Diagnostics;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Purchase : IPurchase
    {
        public Purchase()
        {
        }

        public bool Posted { get; private set; } = false;
        public void Post() => Posted = true;

        public decimal TotalPrice { get; private set; } = 0;
        public Dictionary<(Guid productId, Guid priceId, TransactionType type), uint> Items { get; private set; } = new();
        public bool TryAddItem(Guid productId, Guid priceId, TransactionType type, uint itemCount)
        {
            if (!Posted)
            {
                Items.Add((productId, priceId, type), itemCount);

                //TODO: Find price
                Price price = new(0, 0);

                if (type == TransactionType.Sell)
                {
                    TotalPrice = decimal.Add(TotalPrice, price.SellValue);
                }
                else if (type == TransactionType.Disposed)
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
    }
}