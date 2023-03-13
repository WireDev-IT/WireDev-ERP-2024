namespace WireDev.Erp.V1.Models.Statistics
{
    public class DayStats : TimeStats
    {
        public DayStats() { }

        public DayStats(DateTime date)
        {
            DateTime t = new(date.Year, date.Month, date.Day);
            Date = t.Ticks;
        }

        public override long Date { get; }
    }
}