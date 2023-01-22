using System;
namespace WireDev.Erp.V1.Models.Statistics
{
    public class YearStats : TimeStats
    {
        public YearStats() { }

        public YearStats(DateTime date)
        {
            DateTime t = new(date.Year, 1, 1);
            this.Date = t.Ticks;
        }

        public override long Date { get; }
    }
}