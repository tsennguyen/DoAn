namespace Shopping_Laptop.Models
{
    public class CartItemModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingCost { get; set; }

        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
        public decimal GrandTotal { get; set; }
  
        public decimal Total
        {
            get
            {
                return Quantity * Price;
            }
        }

        public string Image { get; set; } = string.Empty;

        public CartItemModel()
        {

        }



        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Price = product.Price;
            Quantity = 1;
            Image = product.Image;

        }
    }
}
