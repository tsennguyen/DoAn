using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models
{
    public class WishlistModel
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for the user
        public string UserId { get; set; } = string.Empty;

        // Foreign key for the product
        public int ProductId { get; set; }
        public ProductModel Product { get; set; } = null!;
    }
}
