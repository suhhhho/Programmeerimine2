namespace KooliProjekt.WpfApp.Api
{
    public interface IApiClient
    {
        Task<IList<Car>> List();
        Task Save(Car car);
        Task Delete(int id);
    }
}