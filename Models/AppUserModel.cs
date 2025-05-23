using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Shopping_Laptop.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Token { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;

        // New properties for user profile
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? HeightCm { get; set; } // Height in centimeters
        public double? WeightKg { get; set; } // Weight in kilograms
        public string? ShoeSize { get; set; } // Can be string to accommodate various sizing systems (US, EU, UK)

        // Thêm thuộc tính SelectedRoles để sử dụng trong View
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
