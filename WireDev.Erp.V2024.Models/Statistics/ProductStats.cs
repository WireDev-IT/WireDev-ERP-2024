using System;
namespace WireDev.Erp.V1.Models.Statistics
{
    public class ProductStats : IProductStats
    {
        public ProductStats(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public uint SoldItems { get; private set; } = 0;
        public uint CanceledItems { get; private set; } = 0;
        public uint RefundedItems { get; private set; } = 0;
        public uint DisposedItems { get; private set; } = 0;

        public uint? AddSells(uint sells) => sells > 0 ? SoldItems += sells : null;
        public uint? AddCanceledItems(uint canceled_items) => canceled_items > 0 ? CanceledItems += canceled_items : null;
        public uint? AddRefundedItems(uint refunded_items) => refunded_items > 0 ? RefundedItems += refunded_items : null;
        public uint? AddDisposedItems(uint disposed_items) => disposed_items > 0 ? DisposedItems += disposed_items : null;
    }
}