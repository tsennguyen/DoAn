using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Laptop.Models
{
    public enum BehaviorType
    {
        Searched,
        AddedToCart,
        Purchased,
        Clicked
    }

    public class UserBehavior
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public AppUserModel User { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
        public ProductModel Product { get; set; } = null!;

        [Required]
        public BehaviorType BehaviorType { get; set; }

        public DateTime Timestamp { get; set; }

        public float? Rating { get; set; }

        public string? SessionId { get; set; }

        public string? UserAgent { get; set; }

        public string? IpAddress { get; set; }

        public UserBehavior()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    public class ProductEntry
    {
        [Key]
        [Column("ProductId")]
        public uint ProductId { get; set; }

        [Key]
        [Column("CoproductId")]
        public uint CoproductId { get; set; }

        public float Label { get; set; }
    }

    public class ProductPrediction
    {
        public float Score { get; set; }
    }
}
