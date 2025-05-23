using System.ComponentModel.DataAnnotations;
namespace Shopping_Laptop.Models
{
    public class ProductDetailsViewModel
    {
        public ProductModel ProductDetails { get; set; } = null!;
        public RatingModel Rating { get; set; } = new RatingModel();

        [Required(ErrorMessage = "Yêu cầu nhập Đánh giá")]
        public string Comment { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yêu cầu nhập Tên")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; } = string.Empty;

    }
}
