using System.ComponentModel.DataAnnotations;

namespace Shopping_Laptop.Models
{
    public class MomoInfoModel
    {
        [Key]
        public int Id { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public string OrderInfo { get; set; } = string.Empty;
        public int ResultCode { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
    }
}
