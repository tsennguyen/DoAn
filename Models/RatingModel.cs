using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Laptop.Models
{
    public class RatingModel
    {
        [Key]
        public int Id { get; set; }
      
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nhận xét")]
        public string Comment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn số sao")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Star { get; set; }

        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
