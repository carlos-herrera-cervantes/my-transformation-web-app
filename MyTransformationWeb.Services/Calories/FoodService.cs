using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Domain.Models;

using Newtonsoft.Json;

namespace MyTransformationWeb.Services.Calories;

public class FoodService(IHttpClientFactory httpClientFactory, ILogger<FoodService> logger) : IFoodService
{
    #region snippet_Properties

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("my-transformation-calories");

    private readonly ILogger<FoodService> _logger = logger;

    #endregion

    #region snippet_Methods

    /// <summary>
    /// This method returns all foods. In case of an error, it logs the error and returns an empty list.
    /// </summary>
    /// <returns>A list of Food</returns>
    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CaloriesApi.BasePath}/foods");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get foods. Status code: {StatusCode}", httpResponse.StatusCode);
            return [];
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<Food>>(content);
    }

    /// <summary>
    /// This method returns a food by its ID. In case of an error, it logs the error and throws an exception.
    /// </summary>
    /// <param name="id">Food ID</param>
    /// <returns>A Food object</returns>
    /// <exception cref="Exception">Returns an exception when the HTTP request fails</exception>
    public async Task<Food> GetAsync(string id)
    {
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CaloriesApi.BasePath}/foods/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get foods. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception($"Failed to get food. Status code: {httpResponse.StatusCode}");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Food>(content);
    }

    /// <summary>
    /// This method creates a new food. In case of an error, it logs the error and throws an exception.
    /// <param name="food">Food object</param>
    /// <returns>A Food object</returns>
    /// <exception cref="Exception">Returns an exception when the HTTP request fails</exception>
    /// </summary>
    public async Task<Food> CreateAsync(FoodCreation food)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var stringContent = new StringContent(JsonConvert.SerializeObject(food), Encoding.UTF8, "application/json");
        using HttpResponseMessage httpResponse = await _httpClient.PostAsync($"{CaloriesApi.BasePath}/foods", stringContent);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to create food. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception($"Failed to create food. Status code: {httpResponse.StatusCode}");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Food>(content);
    }

    /// <summary>
    /// This method updates a food. In case of an error, it logs the error and throws an exception.
    /// <param name="id">Food ID</param>
    /// <param name="food">Food object</param>
    /// <returns>A Food object</returns>
    /// <exception cref="Exception">Returns an exception when the HTTP request fails</exception>
    /// </summary>
    public async Task<Food> UpdateAsync(string id, Food food)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var stringContent = new StringContent(JsonConvert.SerializeObject(food), Encoding.UTF8, "application/json");
        using HttpResponseMessage httpResponse = await _httpClient.PutAsync($"{CaloriesApi.BasePath}/foods/{id}", stringContent);

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to update food. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception($"Failed to update food. Status code: {httpResponse.StatusCode}");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<Food>(content);
    }

    /// <summary>
    /// This method deletes a food. In case of an error, it logs the error and throws an exception.
    /// <param name="id">Food ID</param>
    /// <exception cref="Exception">Returns an exception when the HTTP request fails</exception>
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync($"{CaloriesApi.BasePath}/foods/{id}");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to delete food. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception($"Failed to delete food. Status code: {httpResponse.StatusCode}");
        }
    }

    #endregion
}
