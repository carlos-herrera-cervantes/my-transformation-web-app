using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Calories;

public interface IFoodService
{
    Task<IEnumerable<Food>> GetAllAsync();

    Task<Food> GetAsync(string id);

    Task<Food> CreateAsync(FoodCreation food);

    Task<Food> UpdateAsync(string id, Food food);

    Task DeleteAsync(string id);
}
