using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models
{
    public class ShippingModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Ward { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int shipping_Fee { get; set; }
    }
}
