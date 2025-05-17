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
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Car>>("");
                return result ?? new List<Car>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении списка автомобилей: {ex.Message}");
                return new List<Car>();
            }
        }

        public async Task Save(Car car)
        {
            try
            {
                if (car.Id == 0)
                {
                    // Для новых объектов используем POST
                    var response = await _httpClient.PostAsJsonAsync("", car);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    // Для существующих объектов используем PUT
                    var response = await _httpClient.PutAsJsonAsync(car.Id.ToString(), car);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при сохранении автомобиля: {ex.Message}");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(id.ToString());
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при удалении автомобиля: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}