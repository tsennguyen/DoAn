using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models
{
    public class UserBehaviorModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public int ProductId { get; set; }

        [Required]
        public string BehaviorType { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        // Navigation properties
        public virtual AppUserModel? User { get; set; }
        public virtual ProductModel? Product { get; set; }
    }
} 