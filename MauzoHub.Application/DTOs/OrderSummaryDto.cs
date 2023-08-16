namespace MauzoHub.Application.DTOs
{
    public class OrderSummaryDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
