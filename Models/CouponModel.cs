using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Tên Mã Khuyến Mãi")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yêu Cầu Tên Mô Tả")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yêu Cầu Mã Giảm Giá")]
        public string Code { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Yêu Cầu Số Lượng Mã Giảm Giá")]
        public int Quantity { get; set; }
        public int Status { get; set; }
    }
}
