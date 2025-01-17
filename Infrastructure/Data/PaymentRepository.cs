using System.Collections.Generic;
using System.Linq;
using PaymentService.Domain.Interfaces;
using PaymentService.Domain.ValueObjects;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly List<Payment> _payments = new();

        public void Add(Payment payment)
        {
            _payments.Add(payment);
        }

        public Payment GetById(int id)
        {
            return _payments.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Payment> GetAllPending()
        {
            return _payments.Where(p => p.Status == PaymentStatus.Pending);
        }

        public void Update(Payment payment)
        {
            var existingPayment = GetById(payment.Id);
            if (existingPayment != null)
            {
                existingPayment.SetStatus(payment.Status);
                existingPayment.HoraFinalizacao = payment.HoraFinalizacao;
            }
        }
    }
}