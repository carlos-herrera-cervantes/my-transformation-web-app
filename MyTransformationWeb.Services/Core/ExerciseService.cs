using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

using MyTransformationWeb.Domain.Models;
using MyTransformationWeb.Domain.Config;

using Newtonsoft.Json;

namespace MyTransformationWeb.Services.Core;

public class ExerciseService(IHttpClientFactory httpClientFactory, ILogger<ExerciseService> logger) : IExerciseService
{
    #region snippet_Properties

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("my-transformation-core");

    private readonly ILogger<ExerciseService> _logger = logger;

    #endregion

    #region snippet_Methods

    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CoreApi.BasePath}/v1/exercise");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get exercises. Status code: {StatusCode}", httpResponse.StatusCode);
            return [];
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<Exercise>>(content);
    }

    public async Task<Exercise> GetAsync(string id)
    {
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CoreApi.BasePath}/v1/exercise/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get exercise. Status code: {StatusCode}", httpResponse.StatusCode);
            return null;
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Exercise>(content);
    }

    public async Task<Exercise> CreateAsync(MultipartFormDataContent multipartFormDataContent)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
        using HttpResponseMessage httpResponse = await _httpClient.PostAsync($"{CoreApi.BasePath}/v1/exercise", multipartFormDataContent);
        string content = string.Empty;

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to create exercise. Status code: {StatusCode}", httpResponse.StatusCode);
            content = await httpResponse.Content.ReadAsStringAsync();
            FailedHttpResponse failedHttpResponse = JsonConvert.DeserializeObject<FailedHttpResponse>(content);
            throw new Exception($"Failed to create exercise: {failedHttpResponse.Message}");
        }

        content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Exercise>(content);
    }

    public async Task<Exercise> UpdateAsync(string id, MultipartFormDataContent multipartFormDataContent)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
        using HttpResponseMessage httpResponse = await _httpClient.PutAsync($"{CoreApi.BasePath}/v1/exercise/{id}", multipartFormDataContent);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to update exercise. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception("Failed to update exercise");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Exercise>(content);
    }

    public async Task DeleteAsync(string id)
    {
        using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"{CoreApi.BasePath}/v1/exercise/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to delete exercise. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception("Failed to delete exercise");
        }
    }

    #endregion
}
