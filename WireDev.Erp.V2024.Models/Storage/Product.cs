using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models.Storage
{
    public class Product<TKey>
    {
        [Key]
        public TKey Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string[]? CategoryId { get; set; }
        public bool Status { get; set; } = false;
        public bool ShowInStorage { get; set; }
    }
}