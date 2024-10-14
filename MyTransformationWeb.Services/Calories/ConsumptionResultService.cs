using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.Extensions.Logging;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Domain.Models;

using Newtonsoft.Json;

namespace MyTransformationWeb.Services.Calories;

public class ConsumptionResultService(IHttpClientFactory httpClientFactory, ILogger<ConsumptionResultService> logger) : IConsumptionResultService
{
    #region snippet_Properties

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("my-transformation-calories");

    private readonly ILogger<ConsumptionResultService> _logger = logger;

    #endregion

    #region snippet_Methods

    public async Task<IEnumerable<ConsumptionResult>> GetAllAsync(string userId, Pageable pageable)
    {
        var uriBuilder = new UriBuilder($"{ExternalServicesConfig.GatewayConfig.CaloriesApiHost}{CaloriesApi.BasePath}/consumptions/result/me");
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
            _logger.LogError("Failed to get consumption results for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            return [];
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<ConsumptionResult>>(content);
    }

    public async Task<ConsumptionResult> GetAsync(string userId, string moment)
    {
        var uriBuilder = new UriBuilder($"{ExternalServicesConfig.GatewayConfig.CaloriesApiHost}{CaloriesApi.BasePath}/consumptions/result/current/me");
        var queryParam = HttpUtility.ParseQueryString(uriBuilder.Query);

        queryParam["date"] = moment;
        uriBuilder.Query = queryParam.ToString();

        var url = uriBuilder.ToString();

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get consumption result for user: {User}. Status code: {StatusCode}.", userId, httpResponse.StatusCode);
            throw new Exception("Failed to get consumption result");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ConsumptionResult>(content);
    }

    public async Task<ConsumptionResult> GetByIdAsync(string userId, string id)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CaloriesApi.BasePath}/consumptions/result/me/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get consumption result for user: {User}. Status code: {StatusCode}.", userId, httpResponse.StatusCode);
            throw new Exception("Failed to get consumption result");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ConsumptionResult>(content);
    }

    public async Task<ConsumptionResult> UpdateAsync(string userId, string id, ConsumptionResult consumptionResult)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var stringContent = new StringContent(JsonConvert.SerializeObject(consumptionResult), Encoding.UTF8, "application/json");
        using HttpResponseMessage httpResponse = await _httpClient.PutAsync($"{CaloriesApi.BasePath}/consumptions/result/me/{id}", stringContent);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to update consumption result for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            throw new Exception("Failed to update consumption result");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ConsumptionResult>(content);
    }

    #endregion
}
