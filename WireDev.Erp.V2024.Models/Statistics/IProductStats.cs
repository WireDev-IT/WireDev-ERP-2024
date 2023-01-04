namespace WireDev.Erp.V1.Models.Statistics
{
    public interface IProductStats
    {
        Guid Id { get; }
        uint SoldItems { get; }
        uint CanceledItems { get; }
        uint RefundedItems { get; }
        uint DisposedItems { get; }

        uint? AddCanceledItems(uint canceled_items);
        uint? AddDisposedItems(uint disposed_items);
        uint? AddRefundedItems(uint refunded_items);
        uint? AddSells(uint sells);
    }
}