using System.Net.Http.Headers;
using Booking.Data;
using Booking.Ivao.DTO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Booking.Ivao.Services
{
    public class AtcPositionService (
        IConfiguration configuration,
        HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string apiKey = configuration["Authentication:APIKey"]!;
        private readonly string baseUrl = configuration["IvaoAPIBaseUrl"]!;
        private async Task<string> GetAtcPositionJson (string position)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("apiKey", apiKey);
                var response = await _httpClient.GetAsync($"baseUrl/v2/positions/search?startsWith={position}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                //Disregard error
            }
            return "";
        }
        public async Task<SearchPositionResult?> GetAtcPosition(string position)
        {
            try
            {
                var json = await GetAtcPositionJson(position);
                var obj = JsonSerializer.Deserialize<List<SearchPositionResult>>(json);
                if (obj is not null)
                {
                    return obj.FirstOrDefault();
                }
            } catch (Exception)
            {

            }
            return null;
        }
        public async Task<List<SearchPositionResult>> GetAtcPositionsList(string position)
        {
            try
            {
                var json = await GetAtcPositionJson(position);
                var obj = JsonSerializer.Deserialize<List<SearchPositionResult>>(json);
                if (obj is not null)
                {
                    return obj;
                }
            }
            catch (Exception)
            {

            }
            return [];

        }
    }
}
