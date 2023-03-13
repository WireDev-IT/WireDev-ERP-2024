namespace WireDev.Erp.V1.Models.Statistics
{
    public class TotalStats : TimeStats
    {
        public TotalStats() { }

        public TotalStats(DateTime date) : base(date)
        {

        }

        public new DateTime GetDate()
        {
            DateTime t = new(Date);
            return t.AddHours(-t.Hour).AddMinutes(-t.Minute).AddSeconds(-t.Second);
        }
    }
}