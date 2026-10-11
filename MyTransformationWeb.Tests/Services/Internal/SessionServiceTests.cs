using System.Diagnostics.CodeAnalysis;
using Moq;
using Xunit;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MyTransformationWeb.Services.Internal;
using MyTransformationWeb.Domain.Models;
using Moq.Protected;

namespace MyTransformationWeb.Tests.Services.Internal;

[Collection("Services")]
[ExcludeFromCodeCoverage]
public class SessionServiceTests
{
    #region snippet_Properties

    private readonly Mock<ILocalStorageService> _localStorageService = new();

    private readonly Mock<NavigationManager> _navigationManager = new() { CallBase = true };

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = $"{nameof(SessionServiceTests)} - {nameof(ValidateSessionShouldInvokeNavigationManager)} - Should invoke navigation manager")]
    public async Task ValidateSessionShouldInvokeNavigationManager()
    {
        _localStorageService
            .Setup(fn => fn.GetItemAsync<User>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Id = string.Empty
            });
        typeof(NavigationManager)
            .GetMethod("Initialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(_navigationManager.Object, ["https://localhost/", "https://localhost/"]);
        _navigationManager
            .Protected()
            .Setup("NavigateToCore", ItExpr.IsAny<string>(), ItExpr.IsAny<bool>())
            .Verifiable();
        var sessionService = new SessionService(_localStorageService.Object, _navigationManager.Object);

        await sessionService.ValidateSession();

        _localStorageService
            .Verify(fn => fn.GetItemAsync<User>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _navigationManager.Protected().Verify("NavigateToCore", Times.Once(), "/", ItExpr.IsAny<bool>());
    }

    #endregion
}
