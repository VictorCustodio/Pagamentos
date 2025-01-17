namespace PaymentService.Domain.ValueObjects
{
    public static class PaymentStatus
    {
        public const string Pending = "Pending";
        public const string Success = "Success";
        public const string Queued = "Queued";
        public const string Failed = "Failed";
    }
}