using MauzoHub.Domain.Entities;

namespace MauzoHub.Domain.Interfaces
{
    public interface IBusinessCategoryRepository : IRepository<BusinessCategories>
    {
        Task<BusinessCategories> FindByName(string name);
    }
}
