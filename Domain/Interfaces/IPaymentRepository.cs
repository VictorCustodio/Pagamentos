using System.Collections.Generic;
using PaymentService.Domain.Entities;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        void Add(Payment payment);
        Payment GetById(int id);
        IEnumerable<Payment> GetAllPending();
        void Update(Payment payment);
        void SaveCreditCard(CreditCard creditCard);
    }
}