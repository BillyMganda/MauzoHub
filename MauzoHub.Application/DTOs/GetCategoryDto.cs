namespace MauzoHub.Application.DTOs
{
    public class GetCategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
