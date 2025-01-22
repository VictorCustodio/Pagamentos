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
        public PaymentStatus Status { get; private set; } = PaymentStatus.PENDENTE;

        // Dados do cartão
        public CreditCard Cartao { get; set; }

        public void FinalizePayment()
        {
            Status = PaymentStatus.PAGA;
            HoraFinalizacao = DateTime.UtcNow;
        }

        public void SetAsFailed()
        {
            Status = PaymentStatus.FALHA;
        }

        public void SetStatus(PaymentStatus status)
        {
            Status = status;
        }
    }
}
