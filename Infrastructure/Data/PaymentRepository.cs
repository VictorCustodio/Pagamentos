using PaymentService.Domain.Entities;
using PaymentService.Domain.Interfaces;
using PaymentService.Domain.ValueObjects;
using PaymentService.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;

namespace PaymentService.Infrastructure.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public void SaveCreditCard(CreditCard creditCard)
        {
            _context.CreditCards.Add(creditCard);
            _context.SaveChanges();
        }

        public Payment GetById(int id)
        {
            return _context.Payments.Find(id);
        }

        public IEnumerable<Payment> GetAllPending()
        {
            return _context.Payments.Where(p => p.Status == PaymentStatus.PENDENTE).ToList();
        }

        public void Update(Payment payment)
        {
            _context.Payments.Update(payment);
            _context.SaveChanges();
        }
    }
}
