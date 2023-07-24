namespace MauzoHub.Infrastructure.Helpers
{
    public class RefreshTokens
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
