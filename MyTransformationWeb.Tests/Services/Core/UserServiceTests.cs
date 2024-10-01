using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.Logging;
using Xunit;

using MyTransformationWeb.Services.Core;
using MyTransformationWeb.Domain.Models;

using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace MyTransformationWeb.Tests.Services.Core;

[Collection("Services")]
[ExcludeFromCodeCoverage]
public class UserServiceTests
{
    #region snippet_Properties

    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();

    private readonly Mock<ILogger<UserService>> _logger = new();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = $"{nameof(UserServiceTests)} - {nameof(GetByEmailAddressAsyncShouldThrowAnException)} - Should throw an exception")]
    public async Task GetByEmailAddressAsyncShouldThrowAnException()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:5001"),
        };

        _httpClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .Verifiable();

        var userService = new UserService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => userService.GetByEmailAddressAsync(emailAddress: "user@example.com"));
    }

    [Fact(DisplayName = $"{nameof(UserServiceTests)} - {nameof(GetByEmailAddressAsyncShouldReturnUser)} - Should return user")]
    public async Task GetByEmailAddressAsyncShouldReturnUser()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:5001"),
        };

        _httpClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new User { Email = "user@example.com" })),
            })
            .Verifiable();

        var userService = new UserService(_httpClientFactory.Object, _logger.Object);
        User user = await userService.GetByEmailAddressAsync(emailAddress: "user@example.com");

        Assert.NotNull(user);
        Assert.Equal("user@example.com", user.Email);
    }

    [Fact(DisplayName = $"{nameof(UserServiceTests)} - {nameof(UpdateAsyncShouldThrowException)} - Should throw an exception")]
    public async Task UpdateAsyncShouldThrowException()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:5001"),
        };

        _httpClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .Verifiable();

        var userService = new UserService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => userService.UpdateAsync(userId: "66b0543812e90c73ece840a3", new()));
    }

    [Fact(DisplayName = $"{nameof(UserServiceTests)} - {nameof(UpdateAsyncShouldReturnUser)} - Should return user")]
    public async Task UpdateAsyncShouldReturnUser()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("https://localhost:5001"),
        };

        _httpClientFactory.Setup(cf => cf.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new User { Email = "user@example.com" })),
            })
            .Verifiable();

        var userService = new UserService(_httpClientFactory.Object, _logger.Object);
        User user = await userService.UpdateAsync(userId: "66b0543812e90c73ece840a3", new());

        Assert.NotNull(user);
        Assert.Equal("user@example.com", user.Email);
    }

    #endregion
}
