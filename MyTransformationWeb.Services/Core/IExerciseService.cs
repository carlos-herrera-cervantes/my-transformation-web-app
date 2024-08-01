using MyTransformationWeb.Domain.Models;

namespace MyTransformationWeb.Services.Core;

public interface IExerciseService
{
    Task<IEnumerable<Exercise>> GetAllAsync();

    Task<Exercise> GetAsync(string id);

    Task<Exercise> CreateAsync(MultipartFormDataContent multipartFormDataContent);

    Task<Exercise> UpdateAsync(string id, MultipartFormDataContent multipartFormDataContent);

    Task DeleteAsync(string id);
}
