namespace MauzoHub.Application.DTOs
{
    public class ErrorLog
    {
        public DateTime DateTime { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
    }
}
