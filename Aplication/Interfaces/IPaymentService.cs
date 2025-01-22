namespace PaymentService.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PaymentService.Domain.Entities;
    using PaymentService.Domain.ValueObjects;

    public interface IPaymentService
    {
        Task<bool> ValidateCardAsync(CreditCard card); // Método para validar Cartão
        Task<Payment> CreatePaymentAsync(PaymentStart paymentStart); // Método para criar uma nova cobrança
        Task<IEnumerable<Payment>> ProcessPendingPaymentsAsync(); // Método para processar pagamentos pendentes
        Payment AddToQueue(PaymentStart paymentStart); // Método síncrono para adicionar à fila
        Payment GetPaymentById(int id); // Método para buscar uma cobrança pelo ID
    }
}
