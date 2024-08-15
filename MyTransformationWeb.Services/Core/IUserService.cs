using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Core;

public interface IUserService
{
    Task<User> GetByEmailAddressAsync(string emailAddress);

    Task<User> UpdateAsync(string userId, User user);
}
