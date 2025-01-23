using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NaLib.CoreService.Lib.Dto;
using NaLib.CoreService.Lib.Services;
using NaLib.CoreService.Lib.Interfaces;
using NaLib.CoreService.Lib.Common;

namespace NaLib.CoreService.Lib.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CatalogService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CatalogResourceDto> GetResourceDetailsAsync(string resourceId)
        {
            var apiUrl = $"{_configuration["LibraryService:BaseUrl"]}/api/v1/LibraryResource/getLibraryResource?id={resourceId}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Body: {responseBody}");

                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var apiResponse = JsonSerializer.Deserialize<Response<CatalogResourceDto>>(responseBody, options);
                        return apiResponse?.Data;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateResourceStatusAsync(string resourceId, string status)
        {
            var getApiUrl = $"{_configuration["LibraryService:BaseUrl"]}/api/v1/LibraryResource/getLibraryResource?id={resourceId}";
            var updateApiUrl = $"{_configuration["LibraryService:BaseUrl"]}/api/v1/LibraryResource/updateLibraryResource?id={resourceId}";

            try
            {
                var getResponse = await _httpClient.GetAsync(getApiUrl);
                getResponse.EnsureSuccessStatusCode();
                var responseBody = await getResponse.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponse = JsonSerializer.Deserialize<Response<UpdateLibraryResourceDto>>(responseBody, options);
                var existingResource = apiResponse?.Data;

                if (existingResource == null)
                {
                    Console.WriteLine("Failed to retrieve existing resource");
                    return false;
                }

                existingResource.BorrowStatus = status;
                existingResource.Id = resourceId;
                var updateResponse = await _httpClient.PutAsJsonAsync(updateApiUrl, existingResource);

                if (updateResponse.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorBody = await updateResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Update failed. Status: {updateResponse.StatusCode}, Error: {errorBody}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }
    }
}

