using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<IList<Car>>> List()
        {
            try
            {
                var cars = await _httpClient.GetFromJsonAsync<List<Car>>("Cars");
                return Result.Ok<IList<Car>>(cars ?? new List<Car>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting list of cars: {ex.Message}");
                return Result.Fail<IList<Car>>(ex);
            }
        }

        public async Task<Result<Car>> GetById(int id)
        {
            try
            {
                var car = await _httpClient.GetFromJsonAsync<Car>($"Cars/{id}");
                if (car == null)
                    return Result.Fail<Car>("Car not found");

                return Result.Ok(car);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting car: {ex.Message}");
                return Result.Fail<Car>(ex);
            }
        }

        public async Task<Result> Save(Car car)
        {
            try
            {
                HttpResponseMessage response;
                if (car.Id == 0)
                {
                    // For new objects use POST
                    response = await _httpClient.PostAsJsonAsync("Cars", car);
                }
                else
                {
                    // For existing objects use PUT
                    response = await _httpClient.PutAsJsonAsync($"Cars/{car.Id}", car);
                }

                if (response.IsSuccessStatusCode)
                {
                    return Result.Ok();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Handle validation errors from server
                    try
                    {
                        var validationProblems = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
                        if (validationProblems != null && validationProblems.Count > 0)
                        {
                            return Result.Fail(validationProblems);
                        }
                    }
                    catch
                    {
                        // If we can't parse the validation errors, fall back to a general message
                    }

                    return Result.Fail("Invalid data submitted");
                }
                else
                {
                    return Result.Fail($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving car: {ex.Message}");
                return Result.Fail(ex);
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Cars/{id}");
                response.EnsureSuccessStatusCode();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting car: {ex.Message}");
                return Result.Fail(ex);
            }
        }
    }
}