using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IRentService
    {
        Task<PagedResult<Rent>> List(int page, int pageSize);
        Task<Rent> Get(int id);
        Task Save(Rent list);
        Task Delete(int id);
    }
}
