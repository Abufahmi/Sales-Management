using Sales.Library.Entities;
using Sales.Library.Services;

namespace Sales.Context.Contracts;

public interface ICategoryService
{
    Task<ResponseService<Category>> CreateCategoryAsync(Category category);
    Task<ResponseService<bool>> DeleteCategoryByIdAsync(int id);
    Task<ResponseService<bool>> DeleteCategoryListAsync(List<int> ids);
    Task<ResponseService<Category>> GetCategoryByIdAsync(int id);
    Task<ResponseService<Category>> GetCategoryListAsync();
    Task<ResponseService<bool>> UpdateCategoryAsync(Category category);
}
