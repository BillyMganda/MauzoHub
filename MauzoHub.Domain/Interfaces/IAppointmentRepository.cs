using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointments>
    {
        public Task<Appointments> GetAppointmentsForuserAsync(Guid userId);
    }
}
