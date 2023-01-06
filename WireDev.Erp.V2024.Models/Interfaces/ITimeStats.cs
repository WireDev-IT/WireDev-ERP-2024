using System;
namespace WireDev.Erp.V1.Models.Statistics
{
	public interface ITimeStats
	{
        long Date { get; }
        decimal Revenue { get; }
        decimal Losses { get; }
        uint SoldItems { get; }
        uint CanceledItemSells { get; }
        uint RefundedItemSells { get; }
        uint DisposedItems { get; }

        uint? AddCanceledItems(uint canceled_items);
        uint? AddDisposedItems(uint disposed_items);
        decimal? AddLosses(decimal losses);
        uint? AddRefundedItems(uint refunded_items);
        decimal? AddRevenue(decimal revenue);
        uint? AddSells(uint sells);
        DateTime GetDate();
    }
}