using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]

        public List<BasketItemDto> Items { get; set; } = null!;

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
