namespace MauzoHub.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }        
        public Guid UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public PaymentDto Payment { get; set; } = new PaymentDto();
        public ShippingAddressDto ShippingAddress { get; set; } = new ShippingAddressDto();
    }
}
