using System;
using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models.ViewModels
{
    public class UpdateAccountViewModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Nghề nghiệp")]
        public string? Occupation { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Chiều cao (cm)")]
        [Range(50, 300, ErrorMessage = "Chiều cao không hợp lệ.")]
        public double? HeightCm { get; set; }

        [Display(Name = "Cân nặng (kg)")]
        [Range(1, 500, ErrorMessage = "Cân nặng không hợp lệ.")]
        public double? WeightKg { get; set; }

        [Display(Name = "Cỡ giày")]
        public string? ShoeSize { get; set; }

        // You can add other fields like Address, PhoneNumber if they are also updatable here
        // For password change, it's often better to have a separate dedicated form/action.
    }
}