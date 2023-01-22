using System;
using System.ComponentModel.DataAnnotations;
using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Interfaces;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Models.Statistics
{
    public class ProductStats
    {
        public ProductStats() { }

        public ProductStats(uint productId)
        {
            ProductId = productId;
        }

        [Key]
        public uint ProductId { get; }
        public Dictionary<long, (TransactionItem, TransactionType)> Transactions { get; private set; } = new();

        public void AddTransaction(TransactionItem item, TransactionType type)
        {
            Transactions.Add(DateTime.UtcNow.Ticks, (item, type));
        }
    }
}