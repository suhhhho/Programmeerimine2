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

        public async Task<Result<IList<Car>>> List()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Car>>("");
                return Result.Ok<IList<Car>>(result ?? new List<Car>());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting list of cars: {ex.Message}");
                return Result.Fail<IList<Car>>(ex);
            }
        }

        public async Task<Result> Save(Car car)
        {
            try
            {
                if (car.Id == 0)
                {
                    // For new objects use POST
                    var response = await _httpClient.PostAsJsonAsync("", car);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    // For existing objects use PUT
                    var response = await _httpClient.PutAsJsonAsync(car.Id.ToString(), car);
                    response.EnsureSuccessStatusCode();
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving car: {ex.Message}");
                return Result.Fail(ex);
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(id.ToString());
                response.EnsureSuccessStatusCode();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting car: {ex.Message}");
                return Result.Fail(ex);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}