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
public class UserProgressServiceTests
{
    #region snippet_Properties

    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();

    private readonly Mock<ILogger<UserProgressService>> _logger = new();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(GetAllAsyncShouldReturnEmptyEnumerable)} - Should return empty enumerable")]
    public async Task GetAllAsyncShouldReturnEmptyEnumerable()
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

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<UserEvolution> progress = await userProgressService.GetAllAsync(userId: "66b0543812e90c73ece840a3", pageable: new Pageable());

        Assert.Empty(progress);
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(GetAllAsyncShouldReturnEnumerable)} - Should return enumerable")]
    public async Task GetAllAsyncShouldReturnEnumerable()
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
                Content =  new StringContent(JsonConvert.SerializeObject(new List<UserEvolution>
                {
                    new(){ UserId = "66b0543812e90c73ece840a3" }
                }))
            })
            .Verifiable();

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<UserEvolution> progress = await userProgressService.GetAllAsync(userId: "66b0543812e90c73ece840a3", pageable: new Pageable());

        Assert.NotEmpty(progress);
        Assert.Equal("66b0543812e90c73ece840a3", progress.First().UserId);
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(GetByIdAsyncShouldThrowException)} - Should throw exception")]
    public async Task GetByIdAsyncShouldThrowException()
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

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        await Assert.ThrowsAsync<Exception>(() => userProgressService.GetByIdAsync(userId: "66b0543812e90c73ece840a3", id: "66b057598e93d8d3722629b4"));
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(GetByIdAsyncShouldReturnUserProgress)} - Should return user progress")]
    public async Task GetByIdAsyncShouldReturnUserProgress()
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
                Content = new StringContent(JsonConvert.SerializeObject(new UserEvolution { UserId = "66b0543812e90c73ece840a3" }))
            })
            .Verifiable();

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        UserEvolution progress = await userProgressService.GetByIdAsync(userId: "66b0543812e90c73ece840a3", id: "66b057598e93d8d3722629b4");

        Assert.NotNull(progress);
        Assert.Equal("66b0543812e90c73ece840a3", progress.UserId);
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(CreateAsyncShouldThrowException)} - Should throw exception")]
    public async Task CreateAsyncShouldThrowException()
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

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        await Assert.ThrowsAsync<Exception>(() => userProgressService.CreateAsync(userId: "66b0543812e90c73ece840a3", new UserEvolution()));
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(CreateAsyncShouldReturnUserProgress)} - Should return user progress")]
    public async Task CreateAsyncShouldReturnUserProgress()
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
                Content = new StringContent(JsonConvert.SerializeObject(new UserEvolution { UserId = "66b0543812e90c73ece840a3" }))
            })
            .Verifiable();

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        UserEvolution progress = await userProgressService.CreateAsync(userId: "66b0543812e90c73ece840a3", new UserEvolution());

        Assert.NotNull(progress);
        Assert.Equal("66b0543812e90c73ece840a3", progress.UserId);
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(UpdateAsyncShouldThrowException)} - Should throw exception")]
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

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        await Assert.ThrowsAsync<Exception>(()
            => userProgressService.UpdateAsync(userId: "66b0543812e90c73ece840a3", id: "66b057598e93d8d3722629b4", new UserEvolution()));
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(UpdateAsyncShouldReturnUserProgress)} - Should return user progress")]
    public async Task UpdateAsyncShouldReturnUserProgress()
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
                Content = new StringContent(JsonConvert.SerializeObject(new UserEvolution { UserId = "66b0543812e90c73ece840a3" }))
            })
            .Verifiable();

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        UserEvolution progress = await userProgressService.UpdateAsync(userId: "66b0543812e90c73ece840a3", id: "66b057598e93d8d3722629b4", new UserEvolution());

        Assert.NotNull(progress);
        Assert.Equal("66b0543812e90c73ece840a3", progress.UserId);
    }

    [Fact(DisplayName = $"{nameof(UserProgressServiceTests)} - {nameof(DeleteAsyncShouldInvokeMethod)} - Should invoke method")]
    public async Task DeleteAsyncShouldInvokeMethod()
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

        var userProgressService = new UserProgressService(_httpClientFactory.Object, _logger.Object);
        await userProgressService.DeleteAsync(userId: "66b0543812e90c73ece840a3", id: "66b057598e93d8d3722629b4");
    }

    #endregion
}
