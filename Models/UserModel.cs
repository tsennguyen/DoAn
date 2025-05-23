using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Shopping_Laptop.Models
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage ="Vui long nhap User")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui long nhap Email"),EmailAddress]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password),Required(ErrorMessage ="Vui long nhap Password")]
        public string Password { get; set; } = string.Empty;

        public string RoleId { get; set; } = string.Empty;
    }
}
