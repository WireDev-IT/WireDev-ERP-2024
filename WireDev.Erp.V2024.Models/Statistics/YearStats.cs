using System;
namespace WireDev.Erp.V1.Models.Statistics
{
    public class YearStats : TimeStats
    {
        public YearStats() { }

        public YearStats(DateTime date) : base(date)
        {
            Date = GetDate().Ticks;
        }

        public override long Date { get; }

        public new DateTime GetDate()
        {
            DateTime t = new(Date);
            return t.AddHours(-t.Hour).AddMinutes(-t.Minute).AddSeconds(-t.Second).AddMonths(-t.Month+1).AddDays(-t.Day+1);
        }
    }
}