using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.Extensions.Logging;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Domain.Models;

using Newtonsoft.Json;

namespace MyTransformationWeb.Services.Calories;

public class ConsumptionService(IHttpClientFactory httpClientFactory, ILogger<ConsumptionService> logger) : IConsumptionService
{
    #region snippet_Properties

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("my-transformation-calories");

    private readonly ILogger<ConsumptionService> _logger = logger;

    #endregion

    #region snippet_Methods

    /// <summary>
    /// This method is used to get all consumptions for a user.
    /// </summary>
    /// <param name="userId">User ID as Object ID in MongoDB</param>
    /// <param name="pageable">Pageable object</param>
    /// <returns>An enumerable of type Consumption</returns>
    public async Task<IEnumerable<Consumption>> GetAllAsync(string userId, Pageable pageable)
    {
        var uriBuilder = new UriBuilder($"{ExternalServicesConfig.GatewayConfig.CaloriesApiHost}{CaloriesApi.BasePath}/consumptions/me");
        var queryParam = HttpUtility.ParseQueryString(uriBuilder.Query);

        queryParam["from"] = pageable.From.ToString("yyyy-MM-ddTHH:mm:ss");
        queryParam["to"] = pageable.To.ToString("yyyy-MM-ddTHH:mm:ss");
        uriBuilder.Query = queryParam.ToString();

        var url = uriBuilder.ToString();

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get consumptions for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            return [];
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<Consumption>>(content);
    }

    /// <summary>
    /// This method is used to get a consumption by ID and user ID.
    /// </summary>
    /// <param name="userId">User ID as Object ID in MongoDB</param>
    /// <param name="id">ID as Object ID in MongoDB</param>
    /// <returns>Consumption object</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Consumption> GetAsync(string userId, string id)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CaloriesApi.BasePath}/consumptions/me/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get consumption for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            throw new Exception("Failed to get consumption");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Consumption>(content);
    }

    /// <summary>
    /// This method is used to create a consumption for a user.
    /// </summary>
    /// <param name="userId">User ID as Object ID in MongoDB</param>
    /// <param name="consumptionCreation">Consumption object</param>
    /// <returns>Consumption object</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Consumption> CreateAsync(string userId, ConsumptionCreation consumptionCreation)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var stringContent = new StringContent(JsonConvert.SerializeObject(consumptionCreation), Encoding.UTF8, "application/json");
        using HttpResponseMessage httpResponse = await _httpClient.PostAsync($"{CaloriesApi.BasePath}/consumptions/me", stringContent);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to create consumption for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            throw new Exception("Failed to create consumption");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Consumption>(content);
    }

    /// <summary>
    /// This method is used to delete a consumption by ID and user ID.
    /// </summary>
    /// <param name="userId">User ID as Object ID in MongoDB</param>
    /// <param name="id">ID as Object ID in MongoDB</param>
    public async Task DeleteAsync(string userId, string id)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"{CaloriesApi.BasePath}/consumptions/me/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to delete consumption for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
        }
    }

    #endregion
}
