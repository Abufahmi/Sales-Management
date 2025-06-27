using Microsoft.EntityFrameworkCore;
using Sales.Context.Contracts;
using Sales.Context.Data;
using Sales.Library.Entities;
using Sales.Library.Services;

namespace Sales.Context.Services;

public class CategoryService(ApplicationDb db) : ICategoryService
{
    public async Task<ResponseService<Category>> CreateCategoryAsync(Category category)
    {
        try
        {
            if (db.Categories.Any(c => c.Name == category.Name))
            {
                return new ResponseService<Category> { IsSuccess = false, ErrorMessage = "Category name exists" };
            }
            await db.AddAsync(category);
            await db.SaveChangesAsync();
            return new ResponseService<Category> { IsSuccess = true, TItem = category };
        }
        catch (Exception ex)
        {
            return new ResponseService<Category> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<ResponseService<bool>> DeleteCategoryByIdAsync(int id)
    {
        var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return new ResponseService<bool> { IsSuccess = false, ErrorMessage = "Category not exists." };
        }
        try
        {
            db.Remove(category);
            await db.SaveChangesAsync();
            return new ResponseService<bool> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseService<bool> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<ResponseService<bool>> DeleteCategoryListAsync(List<int> ids)
    {
        int count = 0;
        try
        {
            foreach (int id in ids)
            {
                var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category != null)
                {
                    db.Remove(category);
                    count++;
                }
            }

            if (count > 0)
            {
                await db.SaveChangesAsync();
            }
            return new ResponseService<bool> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseService<bool> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }

    public async Task<ResponseService<Category>> GetCategoryByIdAsync(int id)
    {
        var category = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return new ResponseService<Category> { IsSuccess = false, ErrorMessage = "Category not exists." };
        }
        return new ResponseService<Category> { IsSuccess = true, TItem = category };
    }

    public async Task<ResponseService<Category>> GetCategoryListAsync()
    {
        var categories = await db.Categories.ToListAsync();
        return new ResponseService<Category> { IsSuccess = categories != null, TList = categories };
    }

    public async Task<ResponseService<bool>> UpdateCategoryAsync(Category category)
    {
        try
        {
            if (db.Categories.Any(c => c.Name == category.Name && c.Id != category.Id))
            {
                return new ResponseService<bool> { IsSuccess = false, ErrorMessage = "Category name exists" };
            }
            db.Update(category);
            await db.SaveChangesAsync();
            return new ResponseService<bool> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseService<bool> { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
