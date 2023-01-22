using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Interfaces
{
    public interface IProductStats
    {
        /// <summary>
        /// The UUID of the product that was transacted.
        /// </summary>
        uint ProductId { get; }

        /// <summary>
        /// The UUID of the price at which this product was transacted.
        /// </summary>
        Guid PriceId { get; }

        /// <summary>
        /// A list of a transactions done with this product.
        /// </summary>
        Dictionary<long, TransactionType> Transactions { get; }

        /// <summary>
        /// Adds a transaction to the statistics of this product.
        /// </summary>
        /// <param name="type">The type of the transaction.</param>
        void AddTransaction(TransactionType type);
    }
}