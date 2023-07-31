namespace MauzoHub.Application.DTOs
{
    public class GetAppointmentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }        
    }
}
