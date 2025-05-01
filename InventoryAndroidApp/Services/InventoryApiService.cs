using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InventoryAndroidApp.Exceptions;
using InventoryAndroidApp.Models;

namespace InventoryAndroidApp.Services
{
    public class InventoryApiService : IInventoryApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private const string BaseRoute = "api/items";

        public InventoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
            _httpClient.BaseAddress ??= new Uri("http://10.0.2.2:5219");

            if (!_httpClient.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")))
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            Debug.WriteLine($"HttpClient configured with BaseAddress: {_httpClient.BaseAddress}");
        }

        public async Task<List<InventoryItem>> GetAllItemsAsync()
        {
            try
            {
                Debug.WriteLine($"Requesting all items from {BaseRoute}");

                var response = await _httpClient.GetAsync(BaseRoute);
                var result = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(result)
                    ? JsonSerializer.Deserialize<List<InventoryItem>>(result, _serializerOptions)
                    : new List<InventoryItem>();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"GetAllItemsAsync failed: {ex.Message}");
                throw new ApiException($"Unable to fetch items: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error in GetAllItemsAsync: {ex}");
                throw new ApiException($"Unexpected error: {ex.Message}");
            }
        }
        public async Task<InventoryItem> CreateItemAsync(InventoryItem item)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(item, _serializerOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(BaseRoute, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(responseBody, _serializerOptions);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"CreateItemAsync failed: {ex.Message}");
                throw new ApiException($"Failed to create item: {ex.Message}");
            }
        }

        public async Task<InventoryItem> UpdateItemAsync(Guid id, InventoryItem item)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(item, _serializerOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{BaseRoute}/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(responseBody, _serializerOptions);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"UpdateItemAsync failed: {ex.Message}");
                throw new ApiException($"Failed to update item: {ex.Message}");
            }
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseRoute}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"DeleteItemAsync failed: {ex.Message}");
                throw new ApiException($"Failed to delete item: {ex.Message}");
            }
        }
    }
}
