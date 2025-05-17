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
            // Укажите правильный базовый адрес вашего API
            _httpClient.BaseAddress = new Uri("https://localhost:7136/api/Rent/");
        }

        public async Task<IList<Rent>> List()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Rent>>("");
            return result;
        }

        public async Task Save(Rent rent)
        {
            if(rent.Id == 0)
            {
                await _httpClient.PostAsJsonAsync("", rent);
            }
            else
            {
                await _httpClient.PutAsJsonAsync(rent.Id.ToString(), rent);
            }
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync(id.ToString());
        }

        public async Task<Rent> Create()
        {
            return new Rent
            {
                Id = 0,
                śtart_time = DateTime.Now,
                end_time = DateTime.Now.AddHours(2),
                kilometrs_driven = 0,
                is_cancelled = false,
                Title = "Новая аренда"
            };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}