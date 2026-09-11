using System.Text.Json;
using EnhanzerAPI.Data;
using EnhanzerAPI.DTOs;
using EnhanzerAPI.DTOs.ExternalApi;
using EnhanzerAPI.Models;

namespace EnhanzerAPI.Services
{
    public interface IAuthService
    {
        Task<AuthServiceResult> LoginAsync(string email, string password);
    }

    public class AuthServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<LocationDetail> Locations { get; set; } = new();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient httpClient, AppDbContext dbContext, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<AuthServiceResult> LoginAsync(string email, string password)
        {
            try
            {
                // Build the external API request
                var request = new ExternalApiLoginRequest
                {
                    API_Action = "GetLoginData",
                    Device_Id = "D001",
                    Sync_Time = "",
                    Company_Code = email,
                    API_Body = new ApiBody
                    {
                        Username = email,
                        Pw = password
                    }
                };

                // Serialize and POST to external API
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(
                    "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke",
                    jsonContent
                );

                if (!response.IsSuccessStatusCode)
                {
                    return new AuthServiceResult
                    {
                        Success = false,
                        Message = $"External API returned status {response.StatusCode}"
                    };
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var externalResponse = JsonSerializer.Deserialize<ExternalApiLoginResponse>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (externalResponse == null || externalResponse.Status_Code != 200 || externalResponse.Response_Body.Count == 0)
                {
                    return new AuthServiceResult
                    {
                        Success = false,
                        Message = externalResponse?.Message ?? "Login failed"
                    };
                }

                // Process locations
                var locations = new List<LocationDetail>();
                var userLocations = externalResponse.Response_Body[0].User_Locations;
                if (userLocations != null)
                {
                    locations = userLocations
                        .Select(ul => new LocationDetail
                        {
                            LocationCode = ul.Location_Code ?? string.Empty,
                            LocationName = ul.Location_Name ?? string.Empty
                        })
                        .ToList();

                    // Clear existing locations and insert new ones
                    _dbContext.LocationDetails.RemoveRange(_dbContext.LocationDetails);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.LocationDetails.AddRange(locations);
                    await _dbContext.SaveChangesAsync();
                }

                return new AuthServiceResult
                {
                    Success = true,
                    Message = "Login successful",
                    Locations = locations
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error during login: {ex.Message}");
                return new AuthServiceResult
                {
                    Success = false,
                    Message = "Network error during authentication"
                };
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON deserialization error: {ex.Message}");
                return new AuthServiceResult
                {
                    Success = false,
                    Message = "Invalid response format from authentication service"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error during login: {ex.Message}");
                return new AuthServiceResult
                {
                    Success = false,
                    Message = "An unexpected error occurred during authentication"
                };
            }
        }
    }
}
