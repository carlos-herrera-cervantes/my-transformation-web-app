using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Calories;

public interface IConsumptionResultService
{
    Task<IEnumerable<ConsumptionResult>> GetAllAsync(string userId, Pageable pageable);

    Task<ConsumptionResult> GetAsync(string userId, string moment);

    Task<ConsumptionResult> GetByIdAsync(string userId, string id);

    Task<ConsumptionResult> UpdateAsync(string userId, string id, ConsumptionResult consumptionResult);
}
