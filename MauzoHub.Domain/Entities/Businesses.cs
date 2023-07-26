namespace MauzoHub.Domain.Entities
{
    public class Businesses : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid OwnerId { get; set; }
    }
}
