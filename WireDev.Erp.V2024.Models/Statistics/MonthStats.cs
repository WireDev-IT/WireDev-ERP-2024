using System;
namespace WireDev.Erp.V1.Models.Statistics
{
    public class MonthStats : TimeStats
    {
        public MonthStats() { }

        public MonthStats(DateTime date)
        {
            DateTime t = new(date.Year, date.Month, 1);
            this.Date = t.Ticks;
        }

        public override long Date { get; }
    }
}