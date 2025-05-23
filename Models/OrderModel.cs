namespace Shopping_Laptop.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; }
        public string? PaymentMethod { get; set; }

        public string? CouponCode { get; set; }
        public string UserName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }

        



    }
}
