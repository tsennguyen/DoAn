using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shopping_Laptop.Repository.Validation;

namespace Shopping_Laptop.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Tiêu Đề Website")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu nhập Bản Đồ")]
        public string Map { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu nhập SDT")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yêu cầu nhập thông tin liên hệ")]
        public string Description { get; set; } = string.Empty;
        public string LogoImg { get; set; } = string.Empty;

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
