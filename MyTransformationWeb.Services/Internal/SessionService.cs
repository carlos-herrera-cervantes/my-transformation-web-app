using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Internal;

public class SessionService(ILocalStorageService localStorageService, NavigationManager navigationManager) : ISessionService
{
    #region properties

    private readonly ILocalStorageService _localStorageService = localStorageService;

    private readonly NavigationManager _navigationManager = navigationManager;

    #endregion

    public async Task ValidateSession()
    {
        User user = await _localStorageService.GetItemAsync<User>(key: "user");
        if (user is null || string.IsNullOrEmpty(user.Id))
        {
            _navigationManager.NavigateTo("/");
        }
    }
}
