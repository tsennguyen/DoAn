﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shopping_Laptop.Models;
using Shopping_Laptop.Models.Momo;
using System.Security.Cryptography;
using System.Text;

namespace Shopping_Laptop.Services.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MomoService> _logger;

        public MomoService(IConfiguration configuration, ILogger<MomoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfo orderInfo)
        {
            try
            {
                var partnerCode = _configuration["Momo:PartnerCode"] ?? throw new InvalidOperationException("Momo PartnerCode is not configured");
                var returnUrl = _configuration["Momo:ReturnUrl"] ?? throw new InvalidOperationException("Momo ReturnUrl is not configured");
                var ipnUrl = _configuration["Momo:IpnUrl"] ?? throw new InvalidOperationException("Momo IpnUrl is not configured");
                var accessKey = _configuration["Momo:AccessKey"] ?? throw new InvalidOperationException("Momo AccessKey is not configured");
                var secretKey = _configuration["Momo:SecretKey"] ?? throw new InvalidOperationException("Momo SecretKey is not configured");
                var paymentUrl = _configuration["Momo:PaymentUrl"] ?? throw new InvalidOperationException("Momo PaymentUrl is not configured");

                var requestId = DateTime.UtcNow.Ticks.ToString();
                var extraData = "";

                var rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={orderInfo.Amount}&orderId={orderInfo.OrderId}&orderInfo={orderInfo.OrderDescription}&returnUrl={returnUrl}&ipnUrl={ipnUrl}&extraData={extraData}";

                var signature = ComputeHmacSha256(rawHash, secretKey);

                var request = new MomoCreatePaymentRequestModel
                {
                    PartnerCode = partnerCode,
                    AccessKey = accessKey,
                    RequestId = requestId,
                    Amount = orderInfo.Amount.ToString(),
                    OrderId = orderInfo.OrderId,
                    OrderInfo = orderInfo.OrderDescription,
                    ReturnUrl = returnUrl,
                    IpnUrl = ipnUrl,
                    ExtraData = extraData,
                    Signature = signature
                };

                using var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(paymentUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseContent))
                {
                    throw new InvalidOperationException("Empty response from Momo API");
                }

                var result = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(responseContent);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize Momo API response");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Momo payment request");
                throw;
            }
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentRequestAsync(string orderId, string orderInfo, decimal amount)
        {
            try
            {
                var partnerCode = _configuration["Momo:PartnerCode"] ?? throw new InvalidOperationException("Momo PartnerCode is not configured");
                var returnUrl = _configuration["Momo:ReturnUrl"] ?? throw new InvalidOperationException("Momo ReturnUrl is not configured");
                var ipnUrl = _configuration["Momo:IpnUrl"] ?? throw new InvalidOperationException("Momo IpnUrl is not configured");
                var accessKey = _configuration["Momo:AccessKey"] ?? throw new InvalidOperationException("Momo AccessKey is not configured");
                var secretKey = _configuration["Momo:SecretKey"] ?? throw new InvalidOperationException("Momo SecretKey is not configured");
                var paymentUrl = _configuration["Momo:PaymentUrl"] ?? throw new InvalidOperationException("Momo PaymentUrl is not configured");

                var requestId = DateTime.UtcNow.Ticks.ToString();
                var extraData = "";

                var rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={amount}&orderId={orderId}&orderInfo={orderInfo}&returnUrl={returnUrl}&ipnUrl={ipnUrl}&extraData={extraData}";

                var signature = ComputeHmacSha256(rawHash, secretKey);

                var request = new MomoCreatePaymentRequestModel
                {
                    PartnerCode = partnerCode,
                    AccessKey = accessKey,
                    RequestId = requestId,
                    Amount = amount.ToString(),
                    OrderId = orderId,
                    OrderInfo = orderInfo,
                    ReturnUrl = returnUrl,
                    IpnUrl = ipnUrl,
                    ExtraData = extraData,
                    Signature = signature
                };

                using var client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(paymentUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseContent))
                {
                    throw new InvalidOperationException("Empty response from Momo API");
                }

                var result = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(responseContent);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize Momo API response");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Momo payment request");
                throw;
            }
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            try
            {
                var partnerCode = collection["partnerCode"].ToString();
                var orderId = collection["orderId"].ToString();
                var requestId = collection["requestId"].ToString();
                var amount = collection["amount"].ToString();
                var orderInfo = collection["orderInfo"].ToString();
                var orderType = collection["orderType"].ToString();
                var transId = collection["transId"].ToString();
                var resultCode = collection["resultCode"].ToString();
                var message = collection["message"].ToString();
                var payType = collection["payType"].ToString();
                var signature = collection["signature"].ToString();

                var secretKey = _configuration["Momo:SecretKey"] ?? throw new InvalidOperationException("Momo SecretKey is not configured");
                var rawHash = $"partnerCode={partnerCode}&accessKey={_configuration["Momo:AccessKey"]}&requestId={requestId}&amount={amount}&orderId={orderId}&orderInfo={orderInfo}&orderType={orderType}&transId={transId}&resultCode={resultCode}&message={message}&payType={payType}";

                var expectedSignature = ComputeHmacSha256(rawHash, secretKey);

                if (signature != expectedSignature)
                {
                    throw new InvalidOperationException("Invalid signature");
                }

                return new MomoExecuteResponseModel
                {
                    PartnerCode = partnerCode,
                    OrderId = orderId,
                    RequestId = requestId,
                    Amount = amount,
                    OrderInfo = orderInfo,
                    OrderType = orderType,
                    TransId = transId,
                    ErrorCode = int.Parse(resultCode),
                    Message = message,
                    PayType = payType,
                    Signature = signature
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing Momo payment");
                throw;
            }
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}