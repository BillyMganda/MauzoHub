namespace MauzoHub.Application.DTOs
{
    public class GetBusinessDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid OwnerId { get; set; }
    }
}
