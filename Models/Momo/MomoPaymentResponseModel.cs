using Newtonsoft.Json;
using System;

namespace Shopping_Laptop.Models.Momo
{
    // Model to represent the data received from MoMo's IPN
    // Refer to MoMo's official documentation for the most accurate and complete list of fields and their data types.
    public class MomoPaymentResponseModel
    {
        [JsonProperty("partnerCode")]
        public string? partnerCode { get; set; }

        [JsonProperty("orderId")]
        public string? orderId { get; set; } // This is MoMo's orderId, not necessarily yours

        [JsonProperty("requestId")]
        public string? requestId { get; set; }

        [JsonProperty("amount")]
        public long? amount { get; set; } // Amount is typically long/number

        [JsonProperty("orderInfo")]
        public string? orderInfo { get; set; }

        [JsonProperty("orderType")]
        public string? orderType { get; set; }

        [JsonProperty("transId")]
        public long? transId { get; set; } // MoMo's transaction ID

        [JsonProperty("resultCode")]
        public int? resultCode { get; set; } // Result code (0 for success)

        [JsonProperty("message")]
        public string? message { get; set; } // Message corresponding to resultCode

        [JsonProperty("payType")]
        public string? payType { get; set; }

        [JsonProperty("responseTime")]
        public long? responseTime { get; set; } // Timestamp

        [JsonProperty("extraData")]
        public string? extraData { get; set; } // Your custom data, should contain your system's OrderCode

        [JsonProperty("signature")]
        public string? signature { get; set; } // Signature for validation
        
        // Add any other fields that MoMo might send in the IPN
        // For example, if there are customer details, discount amounts, etc.
        // public string? customerName { get; set; }
        // public long? discountAmount { get; set; }
    }
} 