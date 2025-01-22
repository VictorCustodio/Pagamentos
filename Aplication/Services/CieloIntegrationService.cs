using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;
using PaymentService.Infrastructure.Configuration;

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
            Console.WriteLine("URL: " + _settings.ApiBaseUrl);
        }

        public async Task<bool> ValidateCardAsync(CreditCard Cartao)
        {
            var payload = new
            {
                CardType = "CreditCard",
                CardNumber = Cartao.Numero,
                Holder = Cartao.NomeTitular,
                ExpirationDate = Cartao.Validade,
                SecurityCode = Cartao.Cvv,
                SaveCard = false,
                Brand = "Visa" // Adaptar conforme necessário
            };
            var response = await _httpClient.PostAsJsonAsync("1/zeroauth/", payload);
            Console.WriteLine("Teste Testado");
            Console.WriteLine(response);
            return response.IsSuccessStatusCode;
            /*try
            {
                var response = await _httpClient.PostAsJsonAsync("1/cardValidation", payload);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<CardValidationResponse>(content);
                    return (result.Valid, result.ReturnMessage);
                }

                return (false, "Erro na validação do cartão: " + content);
            }
            catch (Exception ex)
            {
                return (false, $"Exceção durante a validação do cartão: {ex.Message}");
            }*/
        }

        public async Task<(bool IsSuccessful, string Message)> ChargePaymentAsync(Payment payment)
        {   //request data Ciclista
            var payload = new
            {
                MerchantOrderId = payment.Id.ToString(),
                Payment = new
                {
                    Type = "CreditCard",
                    Amount = (int)(payment.Valor * 100), // Convertendo para centavos
                    Installments = 1,
                    CreditCard = new
                     {
                         CardNumber = payment.Cartao.Numero,
                         Holder = payment.Cartao.NomeTitular,
                         ExpirationDate = payment.Cartao.Validade,
                         SecurityCode = payment.Cartao.Cvv,
                         Brand = "Visa" // Adaptar conforme necessário
                     }
                }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("1/sales", payload);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Cobrança realizada com sucesso.");
                }

                return (false, "Erro ao processar cobrança: " + content);
            }
            catch (Exception ex)
            {
                return (false, $"Exceção ao processar cobrança: {ex.Message}");
            }
        }
    }

    public class CardValidationResponse
    {
        public bool Valid { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public string IssuerTransactionId { get; set; }
    }
}
