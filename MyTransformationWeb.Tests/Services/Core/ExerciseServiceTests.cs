using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Net;
using Xunit;

using MyTransformationWeb.Services.Core;
using MyTransformationWeb.Domain.Models;

using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace MyTransformationWeb.Tests.Services.Core;

[Collection("Services")]
[ExcludeFromCodeCoverage]
public class ExerciseServiceTests
{
    #region snippet_Properties

    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();

    private readonly Mock<ILogger<ExerciseService>> _logger = new();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.GetAllAsyncShouldReturnEmptyEnumerable)} - Should return empty enumerable")]
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

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<Exercise> exercises = await exerciseService.GetAllAsync();

        Assert.Empty(exercises);
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.GetAllAsyncShouldReturnEnumerable)} - Should return enumerable")]
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
                Content = new StringContent(JsonConvert.SerializeObject(new List<Exercise>
                {
                    new(){ Name = "Bench press" }
                }))
            })
            .Verifiable();

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);
        IEnumerable<Exercise> exercises = await exerciseService.GetAllAsync();

        Assert.NotEmpty(exercises);
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.GetAsyncShouldReturnNull)} - Should return null")]
    public async Task GetAsyncShouldReturnNull()
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

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);
        Exercise exercise = await exerciseService.GetAsync("66a935aa4f9d03103a606419");

        Assert.Null(exercise);
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.GetAsyncShouldReturnExercise)} - Should return exercise")]
    public async Task GetAsyncShouldReturnExercise()
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
                Content = new StringContent(JsonConvert.SerializeObject(new Exercise { Name = "Bench press" }))
            })
            .Verifiable();

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);
        Exercise exercise = await exerciseService.GetAsync("66a935aa4f9d03103a606419");

        Assert.NotNull(exercise);
        Assert.True(exercise.Name == "Bench press");
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.CreateAsyncShouldThrowAnException)} - Should throw an exception")]
    public async Task CreateAsyncShouldThrowAnException()
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
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(JsonConvert.SerializeObject(new FailedHttpResponse { Message = "Failed to create exercise" }))
            })
            .Verifiable();

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => exerciseService.CreateAsync(new MultipartFormDataContent()));
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.CreateAsyncShouldReturnExercise)} - Should return exercise")]
    public async Task CreateAsyncShouldReturnExercise()
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
                Content = new StringContent(JsonConvert.SerializeObject(new Exercise { Name = "Bench press" }))
            })
            .Verifiable();

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);
        Exercise exercise = await exerciseService.CreateAsync(new MultipartFormDataContent());

        Assert.NotNull(exercise);
        Assert.True(exercise.Name == "Bench press");
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.UpdateAsyncShouldThrowAnException)} - Should throw an exception")]
    public async Task UpdateAsyncShouldThrowAnException()
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

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => exerciseService.UpdateAsync("66a935aa4f9d03103a606419", new MultipartFormDataContent()));
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.UpdateAsyncShouldReturnExercise)} - Should return exercise")]
    public async Task UpdateAsyncShouldReturnExercise()
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
                Content = new StringContent(JsonConvert.SerializeObject(new Exercise { Name = "Bench press" }))
            })
            .Verifiable();

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);
        Exercise exercise = await exerciseService.UpdateAsync("66a935aa4f9d03103a606419", new MultipartFormDataContent());

        Assert.NotNull(exercise);
        Assert.Equal("Bench press", exercise.Name);
    }

    [Fact(DisplayName = $"{nameof(ExerciseServiceTests)} - {nameof(ExerciseServiceTests.DeleteAsyncShouldThrowAnException)} - Should throw an exception")]
    public async Task DeleteAsyncShouldThrowAnException()
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

        var exerciseService = new ExerciseService(_httpClientFactory.Object, _logger.Object);

        await Assert.ThrowsAsync<Exception>(() => exerciseService.DeleteAsync("66a935aa4f9d03103a606419"));
    }

    #endregion
}
