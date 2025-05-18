using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp.Api
{
    public interface IApiClient
    {
        Task<Result<IList<Car>>> List();
        Task<Result> Save(Car car);
        Task<Result> Delete(int id);
        Task<Result<Car>> GetById(int id);
    }
}