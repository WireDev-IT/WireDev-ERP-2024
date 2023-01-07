using System;
using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Interfaces;

namespace WireDev.Erp.V1.Models.Statistics
{
    public class ProductStats : IProductStats
    {
        public ProductStats(Guid productId, Guid priceId)
        {
            ProductId = productId;
            PriceId = priceId;
            Transactions = new();
        }

        public Guid ProductId { get; }
        public Guid PriceId { get; }
        public Dictionary<long, TransactionType> Transactions { get; private set; }

        public void AddTransaction(TransactionType type)
        {
            Transactions.Add(DateTime.UtcNow.Ticks, type);
        }
    }
}