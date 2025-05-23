using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "PLease enter your name")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "PLease enter your password"), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public bool RememberLogin { get; set; }
    }
}
