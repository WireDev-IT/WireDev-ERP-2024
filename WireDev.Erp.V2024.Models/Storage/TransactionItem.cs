using System;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Storage
{
	public class TransactionItem
	{
        public TransactionItem(uint productId, Guid priceId, uint count)
        {
            ProductId = productId;
            PriceId = priceId;
            Count = count;
        }

        public uint ProductId { get; set; }
		public Guid PriceId { get; set; }
        public uint Count { get; set; }
	}
}