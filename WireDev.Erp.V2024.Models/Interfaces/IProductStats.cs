using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Statistics
{
    public interface IProductStats
    {
        Guid ProductId { get; }
        Guid PriceId { get; }
        uint SoldItems { get; }
        uint CanceledItems { get; }
        uint RefundedItems { get; }
        uint DisposedItems { get; }
        Dictionary<long, TransactionType> Transactions { get; }

        void AddTransaction(TransactionType type);
    }
}