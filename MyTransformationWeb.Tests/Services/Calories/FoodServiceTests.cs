using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.Logging;
using Xunit;

using MyTransformationWeb.Services.Calories;
using MyTransformationWeb.Domain.Models;

using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace MyTransformationWeb.Tests.Services.Calories;

[Collection("Services")]
[ExcludeFromCodeCoverage]
public class FoodServiceTests
{
    #region snippet_Properties

    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();

    private readonly Mock<ILogger<FoodService>> _logger = new();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(GetAllAsyncShouldReturnEmptyEnumerable)} - Should return empty enumerable")]
    public async Task GetAllAsyncShouldReturnEmptyEnumerable()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<Food> food = await foodService.GetAllAsync();

        Assert.Empty(food);
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(GetAllAsyncShouldReturnEnumerable)} - Should return enumerable")]
    public async Task GetAllAsyncShouldReturnEnumerable()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<Food>
                {
                    new(){ Name = "Test Food" }
                })),
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<Food> food = await foodService.GetAllAsync();

        Assert.True(food.First().Name == "Test Food");
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(GetAsyncShouldThrowException)} - Should throw exception")]
    public async Task GetAsyncShouldThrowException()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => foodService.GetAsync("1"));
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(GetAsyncShouldReturnFood)} - Should return food")]
    public async Task GetAsyncShouldReturnFood()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new Food
                {
                    Name = "Test Food"
                })),
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);
        Food food = await foodService.GetAsync("66ef75309e6c22cfed6f8532");

        Assert.True(food.Name == "Test Food");
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(CreateAsyncShouldThrowException)} - Should throw exception")]
    public async Task CreateAsyncShouldThrowException()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => foodService.CreateAsync(new FoodCreation()));
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(CreateAsyncShouldReturnFood)} - Should return food")]
    public async Task CreateAsyncShouldReturnFood()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(new Food
                {
                    Name = "Test Food"
                })),
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);
        Food food = await foodService.CreateAsync(new FoodCreation());

        Assert.True(food.Name == "Test Food");
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(UpdateAsyncShouldThrowException)} - Should throw exception")]
    public async Task UpdateAsyncShouldThrowException()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => foodService.UpdateAsync("66ef75309e6c22cfed6f8532", new Food()));
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(UpdateAsyncShouldReturnFood)} - Should return food")]
    public async Task UpdateAsyncShouldReturnFood()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new Food
                {
                    Name = "Test Food"
                })),
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);
        Food food = await foodService.UpdateAsync("66ef75309e6c22cfed6f8532", new Food());

        Assert.True(food.Name == "Test Food");
    }

    [Fact(DisplayName = $"{nameof(FoodServiceTests)} - {nameof(DeleteAsyncShouldThrowException)} - Should throw exception")]
    public async Task DeleteAsyncShouldThrowException()
    {
        var delegatingHandler = new Mock<DelegatingHandler>();
        var httpClient = new HttpClient(delegatingHandler.Object)
        {
            BaseAddress = new Uri("http://localhost:5001"),
        };

        _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient).Verifiable();
        delegatingHandler.
            Protected().
            Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            })
            .Verifiable();

        var foodService = new FoodService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => foodService.DeleteAsync("66ef75309e6c22cfed6f8532"));
    }

    #endregion
}
