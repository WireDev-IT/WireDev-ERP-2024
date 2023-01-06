using System;
using WireDev.Erp.V1.Models.Interfaces;

namespace WireDev.Erp.V1.Models.Statistics
{
    public class EventOnDate : IEventOnDate
    {
        public EventOnDate(DateTime date, Guid productId)
        {
            Date = date.Ticks;
            ProductId = productId;
        }

        public long Date { get; }
        public Guid ProductId { get; }
        public uint Count { get; private set; }

        public uint AddCount()
        {
            return Count++;
        }
    }
}

