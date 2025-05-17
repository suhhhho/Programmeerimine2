using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<IList<Rent>> List();
        Task Save(Rent rent);
        Task Delete(int id);
        Task<Rent> Create();
    }
}