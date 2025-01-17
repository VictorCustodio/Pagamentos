using System.Threading.Tasks;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using PaymentService.Domain.ValueObjects;

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


        public Payment CreatePayment(Payment payment)
        {
            payment.SetStatus(PaymentStatus.Pending);
            _paymentRepository.Add(payment);
            return payment;
        }

        public Payment AddToQueue(Payment payment)
        {
            payment.SetStatus(PaymentStatus.Queued);
            _paymentRepository.Add(payment);
            return payment;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            if (!await _cieloIntegrationService.ValidateCardAsync(payment))
            {
                payment.SetStatus(PaymentStatus.Failed);
                return payment;
            }

            if (!await _cieloIntegrationService.ChargePaymentAsync(payment))
            {
                payment.SetStatus(PaymentStatus.Failed);
                return payment;
            }

            payment.SetStatus(PaymentStatus.Success);
            _paymentRepository.Add(payment);
            return payment;
        }

        public async Task<IEnumerable<Payment>> ProcessPendingPaymentsAsync()
        {
            var pendingPayments = _paymentRepository.GetAllPending();

            foreach (var payment in pendingPayments)
            {
                if (await _cieloIntegrationService.ChargePaymentAsync(payment))
                {
                    payment.SetStatus(PaymentStatus.Success);
                }
                else
                {
                    payment.SetStatus(PaymentStatus.Failed);
                }

                _paymentRepository.Update(payment);
            }

            return pendingPayments;
        }

        public async Task<bool> ValidateCardAsync(Payment payment)
        {
            return await _cieloIntegrationService.ValidateCardAsync(payment);
        }
    }
}
