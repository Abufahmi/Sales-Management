using Sales.Library.Entities;

namespace Sales.Library.Contracts;

public interface ICategoryService
{
    Task<Category?> CreateCategoryAsync(Category category);
    Task<bool> DeleteCategoryByIdAsync(int id);
    Task<bool> DeleteCategoryListAsync(List<int> ids);
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<List<Category>?> GetCategoryListAsync();
    Task<bool> UpdateCategoryAsync(Category category);
}
