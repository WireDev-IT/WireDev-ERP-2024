using WireDev.Erp.V1.Models.Enums;

namespace WireDev.Erp.V1.Models.Storage
{
    public interface IPurchase
    {
        /// <summary>
        /// The status of the purchase.
        /// </summary>
        bool Posted { get; }

        /// <summary>
        /// The total price of the purchase.
        /// </summary>
        decimal TotalPrice { get; }

        /// <summary>
        /// A list of all products with prices, quantity and their performed transaction of this purchase.
        /// </summary>
        Dictionary<(Guid productId, Guid priceId, TransactionType type), uint> Items { get; }

        /// <summary>
        /// Locks the purchase to prevent changes.
        /// </summary>
        void Post();
        /// <summary>
        /// Attempts to enter the passed purchase data into the sales transaction.
        /// </summary>
        /// <param name="productId">The UUID of the product.</param>
        /// <param name="priceId">The UUID of the price.</param>
        /// <param name="type">The type of transaction of the product.The number of processed products of this transaction.</param>
        /// <param name="itemCount"></param>
        /// <returns>true, if the purchase has not yet been posted. Otherwise false.</returns>
        bool TryAddItem(Guid productId, Guid priceId, TransactionType type, uint itemCount);
    }
}