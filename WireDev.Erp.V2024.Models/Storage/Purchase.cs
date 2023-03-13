using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Purchase
    {
        public Purchase()
        {

        }

        public Purchase(TransactionType type)
        {
            Type = type;
        }

        [Key]
        public Guid Uuid { get; private set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal TotalPrice { get; private set; } = 0;
        public DateTime? DatePosted { get; private set; } = null;
        public TransactionType Type { get; set; }
        public List<TransactionItem> Items { get; private set; } = new();
        public bool Posted { get; private set; } = false;

        public bool TryAddItem(uint productId, Price price, uint itemCount)
        {
            if (!Posted)
            {
                Items.Add(new TransactionItem(productId, price.Uuid, itemCount));

                TotalPrice = Type == TransactionType.Sell
                    ? decimal.Add(TotalPrice, price.SellValue * itemCount)
                    : Type is TransactionType.Disposed or TransactionType.Purchase
                        ? decimal.Subtract(TotalPrice, price.RetailValue * itemCount)
                        : decimal.Subtract(TotalPrice, price.SellValue * itemCount);

                return true;
            }
            return false;
        }
        public void Post()
        {
            Posted = true;
            DatePosted = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
        }
    }
}