using System.ComponentModel.DataAnnotations;

namespace MauzoHub.Domain.DTOs
{
    public class UserRefreshTokens
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
