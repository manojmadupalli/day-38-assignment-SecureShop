
using System.ComponentModel.DataAnnotations;

namespace SecureShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required, StringLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;

        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Navigation
        public Product? Product { get; set; }
    }
}
