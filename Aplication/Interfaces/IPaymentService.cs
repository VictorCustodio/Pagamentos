namespace PaymentService.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PaymentService.Domain.Entities;

    public interface IPaymentService
    {
        Task<bool> ValidateCardAsync(Payment payment); // Método para validar Cartão
        Task<Payment> CreatePaymentAsync(Payment payment); // Método para criar uma nova cobrança
        Task<IEnumerable<Payment>> ProcessPendingPaymentsAsync(); // Método para processar pagamentos pendentes
        Payment AddToQueue(Payment payment); // Método síncrono para adicionar à fila
        Payment GetPaymentById(int id); // Método para buscar uma cobrança pelo ID
    }
}
