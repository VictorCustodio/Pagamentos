namespace PaymentService.Domain.ValueObjects
{
    public enum PaymentStatus
    {
        PENDENTE,  // Pagamento aguardando processamento
        PAGA,      // Pagamento realizado com sucesso
        FALHA,     // Pagamento falhou
        CANCELADA, // Pagamento cancelado
        OCUPADA    // Pagamento sendo processado
    }
}