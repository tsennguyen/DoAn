using System.Collections.Generic;

namespace Shopping_Laptop.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
        public decimal GrandTotal { get; set; }
        public int TotalQuantity { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public int DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
