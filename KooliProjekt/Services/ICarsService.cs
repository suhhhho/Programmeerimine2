using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Threading.Tasks;

public interface ICarsService
{
    Task<PagedResult<Cars>> List(int page, int pageSize, CarsSearch search);
    Task<Cars> Get(int id);
    Task Save(Cars cars);
    Task Delete(int id);
}

