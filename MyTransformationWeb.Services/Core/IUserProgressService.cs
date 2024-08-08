using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Core;

public interface IUserProgressService
{
    public Task<IEnumerable<UserEvolution>> GetAllAsync(string userId, Pageable pageable);

    public Task<UserEvolution> GetByIdAsync(string userId, string id);

    public Task<UserEvolution> CreateAsync(string userId, UserEvolution userEvolution);

    public Task<UserEvolution> UpdateAsync(string userId, string id, UserEvolution userEvolution);

    public Task DeleteAsync(string userId, string id);
}
