namespace WireDev.Erp.V1.Models.Storage
{
    public class DisplayProduct : Product
    {
        public DisplayProduct(uint articelNumber) : base(articelNumber) { }
        public List<DisplayPrice> DisplayPrices { get; set; } = new();
    }
}