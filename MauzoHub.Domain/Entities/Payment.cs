namespace MauzoHub.Domain.Entities
{
    public class Payment
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
    }
}
