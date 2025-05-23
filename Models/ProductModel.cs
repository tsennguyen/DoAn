using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Repository.Validation;

namespace Shopping_Laptop.Models
{
    [Index(nameof(Slug), IsUnique = true)]
    [Index(nameof(Name))]
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Tên Sản phẩm")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Tên sản phẩm phải từ 2 đến 200 ký tự")]
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu nhập Mô tả Sản phẩm")]
        [MinLength(4, ErrorMessage = "Mô tả phải có ít nhất 4 ký tự")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu nhập Giá Sản phẩm")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999999999.99, ErrorMessage = "Giá phải từ 0 đến 999,999,999,999.99")]
        [DisplayFormat(DataFormatString = "{0:#,0} VND", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999999999.99, ErrorMessage = "Giá cũ phải từ 0 đến 999,999,999,999.99")]
        public decimal? OldPrice { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn Thương hiệu")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn một Thương hiệu hợp lệ")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Yêu cầu chọn Danh mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu chọn một Danh mục hợp lệ")]
        public int CategoryId { get; set; }

        public CategoryModel? Category { get; set; }
        public BrandModel? Brand { get; set; }
        public ICollection<RatingModel>? Ratings { get; set; }

        [Required(ErrorMessage = "Yêu cầu có hình ảnh sản phẩm")]
        public string Image { get; set; } = "noimage.png";

        [Required(ErrorMessage = "Yêu cầu nhập số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng đã bán không được âm")]
        public int Sold { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải từ 0% đến 100%")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 5, ErrorMessage = "Đánh giá phải từ 0 đến 5 sao")]
        public decimal Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [NotMapped]
        [FileExtension(ErrorMessage = "Chỉ chấp nhận các file ảnh có định dạng .jpg, .jpeg, .png, .gif")]
        public IFormFile? ImageUpload { get; set; }

        public ProductModel()
        {
            Ratings = new List<RatingModel>();
            Rating = 0;
            Discount = 0;
            Quantity = 0;
            Sold = 0;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
