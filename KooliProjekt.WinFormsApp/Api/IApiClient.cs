using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.WinFormsApp.Api
{
    public interface IApiClient
    {
        Task<Result<IList<Car>>> List();
        Task<Result> Save(Car car);
        Task<Result> Delete(int id);
    }
}