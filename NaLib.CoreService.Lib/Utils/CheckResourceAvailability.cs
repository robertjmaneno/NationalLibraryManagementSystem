using Microsoft.Extensions.Configuration;

public class CheckResourceAvailability
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CheckResourceAvailability(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> IsResourceAvailable(int resourceId)
    {
        var resourceApiUrl = $"{_configuration["ResourceApiBaseUrl"]}/api/resources/check-availability/{resourceId}";
        try
        {
            var response = await _httpClient.GetAsync(resourceApiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return bool.Parse(responseContent);
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to Resource Microservice: {ex.Message}");
            return false;
        }
    }
}
