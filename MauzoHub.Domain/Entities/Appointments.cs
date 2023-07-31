namespace MauzoHub.Domain.Entities
{
    public class Appointments : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }        
    }
}
