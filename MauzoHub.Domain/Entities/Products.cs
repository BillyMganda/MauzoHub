namespace MauzoHub.Domain.Entities
{
    public class Products : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new List<string>();
        public Guid BusinessId { get; set; }
    }
}
