namespace WireDev.Erp.V1.Models.Storage
{
    public class DisplayPrice : Price
    {
        public DisplayPrice(Guid guid) : base(guid) { }

        public string FormattedSellValue => SellValue.ToString("C");
        public string FormattedRetailValue => RetailValue.ToString("C");
    }
}