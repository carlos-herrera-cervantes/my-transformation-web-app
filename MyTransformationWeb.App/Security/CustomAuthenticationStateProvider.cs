using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyTransformationWeb.Domain.Models;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var cachedUser = await _localStorage.GetItemAsync<User>("user");

            if (string.IsNullOrWhiteSpace(cachedUser.Id))
            {
                return new AuthenticationState(_anonymous);
            }

            var claims = new List<Claim>
            {
                new("id", cachedUser.Id),
                new("email", cachedUser.Email)
            };
            var identity = new ClaimsIdentity(claims, "user");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public void MarkUserAsAuthenticated(User user)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new("id", user.Id),
            new("email", user.Email)
        }, "user"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousState = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(anonymousState);
    }
}
