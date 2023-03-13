using System.ComponentModel.DataAnnotations;
using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Interfaces;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Models.Statistics
{
    public class TimeStats : ITimeStats
    {
        public TimeStats() { }

        public TimeStats(DateTime date)
        {
            Date = date.Ticks;
        }

        [Key]
        public virtual long Date { get; }
        public decimal Expenses { get; private set; } = 0;
        public decimal Revenue { get; private set; } = 0;
        public decimal Losses { get; private set; } = 0;
        public uint SoldItems { get; private set; } = 0;
        public uint PurchasedItems { get; private set; } = 0;
        public uint CanceledItems { get; private set; } = 0;
        public uint RefundedItems { get; private set; } = 0;
        public uint DisposedItems { get; private set; } = 0;

        public Task AddTransaction(uint count, Price price, TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Sell:
                    Revenue = decimal.Add(Revenue, count * price.SellValue);
                    SoldItems += count;
                    break;
                case TransactionType.Cancel:
                    Revenue = decimal.Subtract(Revenue, count * price.SellValue);
                    CanceledItems += count;
                    break;
                case TransactionType.Refund:
                    Losses = decimal.Add(Losses, count * price.SellValue);
                    RefundedItems += count;
                    break;
                case TransactionType.Disposed:
                    Losses = decimal.Add(Losses, count * price.RetailValue);
                    DisposedItems += count;
                    break;
                case TransactionType.Purchase:
                    Expenses = decimal.Add(Expenses, count * price.RetailValue);
                    PurchasedItems += count;
                    break;
            }
            return Task.CompletedTask;
        }

        public DateTime GetDate()
        {
            return new DateTime(Date);
        }
    }
}