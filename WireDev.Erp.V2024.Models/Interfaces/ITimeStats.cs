using WireDev.Erp.V1.Models.Enums;
using WireDev.Erp.V1.Models.Storage;

namespace WireDev.Erp.V1.Models.Enums
{
    public interface ITimeStats
    {
        long Date { get; }
        decimal Expenses { get; }
        decimal Revenue { get; }
        decimal Losses { get; }
        uint SoldItems { get; }
        uint PurchasedItems { get; }
        uint CanceledItems { get; }
        uint RefundedItems { get; }
        uint DisposedItems { get; }

        Task AddTransaction(uint count, Price price, TransactionType type);
        DateTime GetDate();
    }
}