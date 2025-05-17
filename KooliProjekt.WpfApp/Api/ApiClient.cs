using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp.Api
{
    public class ApiClient : IApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/Cars/");
        }

        public async Task<IList<Car>> List()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Car>>("");
            return result;
        }

        public async Task Save(Car car)
        {
            if (car.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("", car);
            }
            else
            {
                await _httpClient.PutAsJsonAsync(car.Id.ToString(), car);
            }
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync(id.ToString());
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}