using PaymentService.Domain.ValueObjects;
using System;

namespace PaymentService.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public int Ciclista { get; set; }
        public DateTime HoraSolicitacao { get; private set; } = DateTime.UtcNow;
        public DateTime? HoraFinalizacao { get; set; }
        public string Status { get; private set; } = PaymentStatus.Pending;

        // Dados do cartão
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }

        public void FinalizePayment()
        {
            Status = PaymentStatus.Success;
            HoraFinalizacao = DateTime.UtcNow;
        }

        public void SetAsFailed()
        {
            Status = PaymentStatus.Failed;
        }

        public void SetStatus(string status)
        {
            Status = status;
        }
    }
}