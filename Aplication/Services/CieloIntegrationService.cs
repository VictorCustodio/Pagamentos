using System;
using System.Net.Http;
using System.Threading.Tasks;
using PaymentService.Infrastructure.Configuration;
using PaymentService.Domain.Entities;
using Microsoft.Extensions.Options;


namespace PaymentService.Application.Services
{
    public class CieloIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly CieloSettings _settings;

        public CieloIntegrationService(HttpClient httpClient, IOptions<CieloSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _httpClient.BaseAddress = new Uri(_settings.ApiBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("MerchantId", _settings.MerchantId);
            _httpClient.DefaultRequestHeaders.Add("MerchantKey", _settings.MerchantKey);
        }

        public async Task<bool> ValidateCardAsync(Payment payment)
        {
            var payload = new
            {
                CardNumber = payment.CardNumber,
                CardHolder = payment.CardHolder,
                ExpirationDate = payment.ExpirationDate,
                SecurityCode = payment.CVV
            };

            var response = await _httpClient.PostAsJsonAsync("1/cardValidation", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChargePaymentAsync(Payment payment)
        {
            var payload = new
            {
                MerchantOrderId = payment.Id.ToString(),
                Payment = new
                {
                    Type = "CreditCard",
                    Amount = payment.Valor * 100, // valor em centavos
                    Installments = 1,
                    CreditCard = new
                    {
                        CardNumber = payment.CardNumber,
                        Holder = payment.CardHolder,
                        ExpirationDate = payment.ExpirationDate,
                        SecurityCode = payment.CVV,
                        Brand = "Visa" // Adapte conforme o suporte da Cielo
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("1/sales", payload);
            return response.IsSuccessStatusCode;
        }
    }
}