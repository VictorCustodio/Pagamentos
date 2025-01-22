using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using PaymentService.Domain.ValueObjects;
using System.Text;
using System.Security.Cryptography;

namespace PaymentService.Application.Services
{
    public class PaymentServiceCore : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly CieloIntegrationService _cieloIntegrationService;

        public PaymentServiceCore(IPaymentRepository paymentRepository, CieloIntegrationService cieloIntegrationService)
        {
            _paymentRepository = paymentRepository;
            _cieloIntegrationService = cieloIntegrationService;
        }

        public Payment GetPaymentById(int id)
        {
            return _paymentRepository.GetById(id);
        }

        public Payment CreatePayment(PaymentStart paymentStart)
        {
            Payment payment = new Payment();
            payment.SetStatus(PaymentStatus.PENDENTE); // Atualiza para status pendente
            payment.Valor = paymentStart.Valor;
            payment.Ciclista = paymentStart.Ciclista;
            payment.Cartao = new CreditCard();
            payment.Cartao.Numero = "5234123412341234";
            payment.Cartao.NomeTitular = "Teste";
            payment.Cartao.Validade = "06/2025";
            payment.Cartao.Cvv = "888";
            _paymentRepository.SaveCreditCard(payment.Cartao);
            _paymentRepository.Add(payment);
            return payment;
        }

        public Payment AddToQueue(PaymentStart paymentStart)
        {
            Payment payment = new Payment();
            payment.SetStatus(PaymentStatus.OCUPADA);
            payment.Valor = paymentStart.Valor;
            payment.Ciclista = paymentStart.Ciclista;
            payment.Cartao = new CreditCard();
            payment.Cartao.Numero = "5234123412341234";
            payment.Cartao.NomeTitular = "Teste";
            payment.Cartao.Validade = "06/2025";
            payment.Cartao.Cvv = "888";
            _paymentRepository.SaveCreditCard(payment.Cartao);
            _paymentRepository.Add(payment);
            return payment;
        }

        public async Task<Payment> CreatePaymentAsync(PaymentStart paymentStart)
        {
            /* Validação do cartão
            if (!await _cieloIntegrationService.ValidateCardAsync(payment.Cartao))
            {
                payment.SetStatus(PaymentStatus.FALHA);
                return payment;
            }*/
            Payment payment = new Payment();
            payment.Valor = paymentStart.Valor;
            payment.Ciclista = paymentStart.Ciclista;
            payment.Cartao = new CreditCard();//Erro para corrigir
            payment.Cartao.Numero = "5234123412341234";
            payment.Cartao.NomeTitular = "Teste";
            payment.Cartao.Validade = "06/2025";
            payment.Cartao.Cvv = "888";
            _paymentRepository.SaveCreditCard(payment.Cartao);

            // Cobrança do pagamento
            var chargeResult = await _cieloIntegrationService.ChargePaymentAsync(payment);
            if (!chargeResult.IsSuccessful)
            {
                payment.SetStatus(PaymentStatus.FALHA);
                return payment;
            }

            payment.SetStatus(PaymentStatus.PAGA);
            _paymentRepository.Add(payment);
            return payment;
        }

        public async Task<IEnumerable<Payment>> ProcessPendingPaymentsAsync()
        {
            var pendingPayments = _paymentRepository.GetAllPending();

            foreach (var payment in pendingPayments)
            {
                // Tenta cobrar o pagamento e atualiza o status
                var chargeResult = await _cieloIntegrationService.ChargePaymentAsync(payment);
                if (chargeResult.IsSuccessful)
                {
                    payment.SetStatus(PaymentStatus.PAGA);
                }
                else
                {
                    payment.SetStatus(PaymentStatus.FALHA);
                }

                _paymentRepository.Update(payment);
            }

            return pendingPayments;
        }

        public async Task<bool> ValidateCardAsync(CreditCard card)
        {
            return await _cieloIntegrationService.ValidateCardAsync(card);
        }
    }
}
