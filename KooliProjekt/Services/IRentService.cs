using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IRentService
    {
        Task<PagedResult<Rent>> List(int page, int pageSize, RentSearch search);
        Task<Rent> Get(int id);
        Task Save(Rent list);
        Task Delete(int id);
    }
}
