namespace WireDev.Erp.V1.Models.Interfaces
{
    public interface IEventOnDate
    {
        long Date { get; }
        Guid ProductId { get; }
        uint Count { get; }

        uint AddCount();
    }
}