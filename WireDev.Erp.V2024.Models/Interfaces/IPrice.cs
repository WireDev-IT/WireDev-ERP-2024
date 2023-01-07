namespace WireDev.Erp.V1.Models.Interfaces
{
    public interface IPrice
    {
        /// <summary>
        /// The UUID of the price.
        /// </summary>
        Guid Uuid { get; }

        /// <summary>
        /// Indicates whether this price was archived.
        /// </summary>
        bool Archived { get; set; }

        /// <summary>
        /// The description of the price.
        /// </summary>
        string? Description { get; set; }

        /// <summary>
        /// The price at which a product is purchased.
        /// </summary>
        decimal RetailValue { get; set; }

        /// <summary>
        /// The price at which a product is sold to the customer.
        /// </summary>
        decimal SellValue { get; set; }

        /// <summary>
        /// The status of the price.
        /// </summary>
        bool Locked { get; }

        /// <summary>
        /// Locks the price to prevent changes when it is used elsewhere.
        /// </summary>
        void Lock();
    }
}