using System;
using WireDev.Erp.V1.Models.Interfaces;

namespace WireDev.Erp.V1.Models.Statistics
{
    public class TimeStats : ITimeStats
    {
        public TimeStats() { }

        public TimeStats(DateTime date)
        {
            Date = date.Ticks;
        }

        public long Date { get; }
        public decimal Revenue { get; private set; } = 0;
        public decimal Losses { get; private set; } = 0;
        public uint SoldItems { get; private set; } = 0;
        public uint CanceledItemSells { get; private set; } = 0;
        public uint RefundedItemSells { get; private set; } = 0;
        public uint DisposedItems { get; private set; } = 0;

        public decimal? AddRevenue(decimal revenue) => revenue > decimal.Zero ? Revenue = decimal.Add(Revenue, revenue) : null;
        public decimal? AddLosses(decimal losses) => losses > decimal.Zero ? Losses = decimal.Add(Losses, losses) : null;
        public uint? AddSells(uint sells) => sells > 0 ? SoldItems += sells : null;
        public uint? AddCanceledItems(uint canceled_items) => canceled_items > 0 ? CanceledItemSells += canceled_items : null;
        public uint? AddRefundedItems(uint refunded_items) => refunded_items > 0 ? RefundedItemSells += refunded_items : null;
        public uint? AddDisposedItems(uint disposed_items) => disposed_items > 0 ? DisposedItems += disposed_items : null;

        public DateTime GetDate()
        {
            return new DateTime(Date);
        }

    }
}

