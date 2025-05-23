using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập Tên Thương hiệu")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yêu cầu nhập Mô tả Thương hiệu")]
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public int Status { get; set; }
        
        // Navigation property
        public virtual ICollection<ProductModel>? Products { get; set; }
    }
}
