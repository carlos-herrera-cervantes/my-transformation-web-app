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
public class ConsumptionServiceTests
{
    #region snippet_Properties

    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();

    private readonly Mock<ILogger<ConsumptionService>> _logger = new();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = $"{nameof(ConsumptionServiceTests)} - {nameof(CreateMealAsyncShouldThrowException)} - Should throw exception")]
    public async Task CreateMealAsyncShouldThrowException()
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

        var consumptionService = new ConsumptionService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => consumptionService.CreateMealAsync("66b0543812e90c73ece840a3", []));
    }

    [Fact(DisplayName = $"{nameof(ConsumptionServiceTests)} - {nameof(CreateMealAsyncShouldReturn201StatusCode)} - Should return 201 status code")]
    public async Task CreateMealAsyncShouldReturn201StatusCode()
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
                Content = new StringContent(JsonConvert.SerializeObject(new List<Consumption>
                {
                    new(){ UserId = "66b0543812e90c73ece840a3" }
                })),
            })
            .Verifiable();

        var consumptionService = new ConsumptionService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<Consumption> consumptions = await consumptionService.CreateMealAsync("66b0543812e90c73ece840a3", []);

        Assert.True(consumptions.First().UserId == "66b0543812e90c73ece840a3");
    }

    #endregion
}
