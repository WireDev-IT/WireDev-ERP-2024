using System;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Storage
{
	public class TransactionItem
	{
        public TransactionItem(uint productId, Guid prizeId, TransactionType type, uint count)
        {
            ProductId = productId;
            PriceId = prizeId;
            Type = type;
            Count = count;
        }

        public uint ProductId { get; set; }
		public Guid PriceId { get; set; }
		public TransactionType Type { get; set; }
        public uint Count { get; set; }
	}
}