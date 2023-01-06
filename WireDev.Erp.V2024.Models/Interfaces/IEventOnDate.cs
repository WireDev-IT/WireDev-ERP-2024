namespace WireDev.Erp.V1.Models.Statistics
{
    public interface IEventOnDate
    {
        long Date { get; }
        Guid ProductId { get; }
        uint Count { get; }

        uint AddCount();
    }
}