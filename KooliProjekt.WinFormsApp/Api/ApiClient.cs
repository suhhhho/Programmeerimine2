using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.WinFormsApp.Api
{
    public class ApiClient : IApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public ApiClient(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<Result<IList<Car>>> List()
        {
            try
            {
                var cars = await _httpClient.GetFromJsonAsync<IList<Car>>("api/cars");
                return Result.Ok(cars);
            }
            catch (Exception ex)
            {
                return Result.Fail<IList<Car>>(ex);
            }
        }

        public async Task<Result> Save(Car car)
        {
            try
            {
                HttpResponseMessage response;
                if (car.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync("api/cars", car);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"api/cars/{car.Id}", car);
                }

                response.EnsureSuccessStatusCode();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex);
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/cars/{id}");
                response.EnsureSuccessStatusCode();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}