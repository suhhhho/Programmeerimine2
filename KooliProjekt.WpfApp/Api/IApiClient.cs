using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<Result<IList<Car>>> List();
        Task<Result> Save(Car car);
        Task<Result> Delete(int id);
    }
}