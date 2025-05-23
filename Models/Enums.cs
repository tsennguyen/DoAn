namespace Shopping_Laptop.Models
{
    public enum OrderStatus
    {
        Pending = 1, // Chờ xử lý/thanh toán
        Processing = 2, // Đang xử lý
        Shipped = 3, // Đã giao hàng
        Completed = 4, // Hoàn thành
        Cancelled = 5, // Đã hủy
        Failed = 6 // Thất bại (ví dụ: thanh toán lỗi)
    }

    public static class PaymentMethod
    {
        public const string COD = "COD";
        public const string Momo = "Momo";
        public const string Vnpay = "Vnpay"; // Nếu có dùng
        // Thêm các phương thức khác nếu cần
    }
} 