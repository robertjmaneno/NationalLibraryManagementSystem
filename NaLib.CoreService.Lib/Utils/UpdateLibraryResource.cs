using Microsoft.Extensions.Configuration;
using NaLib.CoreService.Lib.Dto;
using System.Net.Http.Json;

public class UpdateResourceAvailability
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public UpdateResourceAvailability(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> UpdateResourceStatusAsync(string resourceId, string newStatus)
    {
        var apiUrl = $"{_configuration["CatalogService:BaseUrl"]}/api/Catalog/{resourceId}/updateStatus";


        var updateRequest = new
        {
            ResourceId = resourceId,
            BorrowStatus = newStatus
        };


        var response = await _httpClient.PutAsJsonAsync(apiUrl, updateRequest);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}
