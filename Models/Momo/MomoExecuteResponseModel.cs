namespace Shopping_Laptop.Models.Momo
{
    public class MomoExecuteResponseModel
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string OrderInfo { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string TransId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string LocalMessage { get; set; } = string.Empty;
        public string ResponseTime { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
        public string PayType { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
