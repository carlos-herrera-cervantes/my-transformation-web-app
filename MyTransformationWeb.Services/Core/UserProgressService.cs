using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Microsoft.Extensions.Logging;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Domain.Models;

using Newtonsoft.Json;

namespace MyTransformationWeb.Services.Core;

public class UserProgressService(IHttpClientFactory httpClientFactory, ILogger<UserProgressService> logger) : IUserProgressService
{
    #region snippet_Properties

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("my-transformation-core");

    private readonly ILogger<UserProgressService> _logger = logger;

    #endregion

    #region snippet_Methods

    public async Task<IEnumerable<UserEvolution>> GetAllAsync(string userId, Pageable pageable)
    {
        var uriBuilder = new UriBuilder($"{ExternalServicesConfig.GatewayConfig.CoreApiHost}{CoreApi.BasePath}/v1/users/progress/me");
        var queryParam = HttpUtility.ParseQueryString(uriBuilder.Query);

        queryParam["page"] = pageable.Page.ToString();
        queryParam["page_size"] = pageable.PageSize.ToString();
        queryParam["from"] = pageable.From.ToString("yyyy-MM-dd");
        queryParam["to"] = pageable.To.ToString("yyyy-MM-dd");
        uriBuilder.Query = queryParam.ToString();

        var url = uriBuilder.ToString();

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get progress for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            return Enumerable.Empty<UserEvolution>();
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<UserEvolution>>(content);
    }

    public async Task<UserEvolution> GetByIdAsync(string userId, string id)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CoreApi.BasePath}/v1/users/progress/me/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get progress for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            throw new Exception("Failed to get progress");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<UserEvolution>(content);
    }

    public async Task<UserEvolution> CreateAsync(string userId, UserEvolution userEvolution)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var reqBody = new StringContent(JsonConvert.SerializeObject(userEvolution), Encoding.UTF8, "application/json");
        using HttpResponseMessage httpResponse = await _httpClient.PostAsync($"{CoreApi.BasePath}/v1/users/progress/me", reqBody);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to create progress for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            throw new Exception("Failed to create progress");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<UserEvolution>(content);
    }

    public async Task<UserEvolution> UpdateAsync(string userId, string id, UserEvolution userEvolution)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);

        var reqBody = new StringContent(JsonConvert.SerializeObject(userEvolution), Encoding.UTF8, "application/json");
        using HttpResponseMessage httpResponse = await _httpClient.PatchAsync($"{CoreApi.BasePath}/v1/users/progress/me/{id}", reqBody);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to update progress for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
            throw new Exception("Failed to update progress");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<UserEvolution>(content);
    }

    public async Task DeleteAsync(string userId, string id)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-id", userId);
        using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"{CoreApi.BasePath}/v1/users/progress/me/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to delete progress for user: {User}. Status code: {StatusCode}", userId, httpResponse.StatusCode);
        }
    }

    #endregion
}
