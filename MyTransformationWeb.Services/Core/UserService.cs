using Microsoft.Extensions.Logging;

using MyTransformationWeb.Domain.Config;
using MyTransformationWeb.Domain.Models;

using Newtonsoft.Json;

namespace MyTransformationWeb.Services.Core;

public class UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger) : IUserService
{
    #region snippet_Properties

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("my-transformation");

    private readonly ILogger<UserService> _logger = logger;

    #endregion

    #region snippet_Methods

    public async Task<User> GetByEmailAddressAsync(string emailAddress)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("user-email", emailAddress);
        using HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{CoreApi.BasePath}/v1/users/me");

        if (!httpResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get user. Status code: {StatusCode}", httpResponse.StatusCode);
            throw new Exception("Failed to get user");
        }

        string content = await httpResponse.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<User>(content);
    }

    #endregion
}
