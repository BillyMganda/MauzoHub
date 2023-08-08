namespace MauzoHub.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
