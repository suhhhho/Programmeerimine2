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

        // Car methods
        public async Task<Result<IList<Car>>> List()
        {
            try
            {
                var cars = await _httpClient.GetFromJsonAsync<List<Car>>("api/Cars");
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
                var car = await _httpClient.GetFromJsonAsync<Car>($"api/Cars/{id}");
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
                    response = await _httpClient.PostAsJsonAsync("api/Cars", car);
                }
                else
                {
                    // For existing objects use PUT
                    response = await _httpClient.PutAsJsonAsync($"api/Cars/{car.Id}", car);
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
                var response = await _httpClient.DeleteAsync($"api/Cars/{id}");
                response.EnsureSuccessStatusCode();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting car: {ex.Message}");
                return Result.Fail(ex);
            }
        }

        // Invoice methods
        public async Task<Result<IList<Invoice>>> ListInvoices()
        {
            try
            {
                var invoices = await _httpClient.GetFromJsonAsync<List<Invoice>>("api/Invoices");
                return Result.Ok<IList<Invoice>>(invoices ?? new List<Invoice>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting list of invoices: {ex.Message}");
                return Result.Fail<IList<Invoice>>(ex);
            }
        }

        public async Task<Result<Invoice>> GetInvoiceById(int id)
        {
            try
            {
                var invoice = await _httpClient.GetFromJsonAsync<Invoice>($"api/Invoices/{id}");
                if (invoice == null)
                    return Result.Fail<Invoice>("Invoice not found");

                return Result.Ok(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting invoice: {ex.Message}");
                return Result.Fail<Invoice>(ex);
            }
        }

        public async Task<Result> SaveInvoice(Invoice invoice)
        {
            try
            {
                HttpResponseMessage response;
                if (invoice.Id == 0)
                {
                    // For new objects use POST
                    response = await _httpClient.PostAsJsonAsync("api/Invoices", invoice);
                }
                else
                {
                    // For existing objects use PUT
                    response = await _httpClient.PutAsJsonAsync($"api/Invoices/{invoice.Id}", invoice);
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
                Console.WriteLine($"Error saving invoice: {ex.Message}");
                return Result.Fail(ex);
            }
        }

        public async Task<Result> DeleteInvoice(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Invoices/{id}");
                response.EnsureSuccessStatusCode();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting invoice: {ex.Message}");
                return Result.Fail(ex);
            }
        }

        // Rent methods
        public async Task<Result<IList<Rent>>> ListRents()
        {
            try
            {
                var rents = await _httpClient.GetFromJsonAsync<List<Rent>>("api/Rents");
                return Result.Ok<IList<Rent>>(rents ?? new List<Rent>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting list of rents: {ex.Message}");
                return Result.Fail<IList<Rent>>(ex);
            }
        }
    }
}
