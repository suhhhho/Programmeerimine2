using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp.Api
{
    public interface IApiClient
    {
        // Car methods
        Task<Result<IList<Car>>> List();
        Task<Result> Save(Car car);
        Task<Result> Delete(int id);
        Task<Result<Car>> GetById(int id);

        // Invoice methods
        Task<Result<IList<Invoice>>> ListInvoices();
        Task<Result<Invoice>> GetInvoiceById(int id);
        Task<Result> SaveInvoice(Invoice invoice);
        Task<Result> DeleteInvoice(int id);

        // Rent methods
        Task<Result<IList<Rent>>> ListRents();
    }
}
