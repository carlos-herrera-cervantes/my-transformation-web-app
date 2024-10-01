using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Calories;

public interface IConsumptionService
{
    Task<IEnumerable<Consumption>> GetAllAsync(string userId, Pageable pageable);

    Task<Consumption> GetAsync(string userId, string id);

    Task<Consumption> CreateAsync(string userId, ConsumptionCreation consumptionCreation);

    Task DeleteAsync(string userId, string id);
}
