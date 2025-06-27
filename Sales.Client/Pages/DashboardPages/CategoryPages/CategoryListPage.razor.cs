using Microsoft.AspNetCore.Components;
using Sales.Client.Helpers;
using Sales.Client.Models;
using Sales.Client.Services;
using Sales.Library.Contracts;
using Sales.Library.Entities;
using Translate = Sales.Client.Resources.App;

namespace Sales.Client.Pages.DashboardPages.CategoryPages;

public partial class CategoryListPage
{
    #region Properties

    [Inject] ICategoryService CategoryService { get; set; } = default!;
    [Inject] IFileService FileService { get; set; } = default!;

    List<CategoryModel> Categories { get; set; } = [];
    List<CategoryModel> AllCategories { get; set; } = [];
    bool IsBusy { get; set; }
    bool IsAllSelected { get; set; }
    string? ErrorMessage { get; set; }

    #endregion


    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        var categires = await CategoryService.GetCategoryListAsync();
        if (categires != null)
        {
            Categories = ModelConverter.GetCategoryListModels(categires);
            AllCategories = Categories;
        }
        IsBusy = false;
    }

    private void OnChanged(ChangeEventArgs args)
    {
        if (args.Value is not string value)
        {
            Categories = AllCategories;
            return;
        }

        var models = AllCategories
            .Where(c => c.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
            .ToList();

        Categories = models;
    }

    protected async Task ApproveResultAsync(bool args)
    {
        if (args == false)
        {
            return;
        }

        IsBusy = true;
        var categories = Categories
             .Where(x => x.IsSelected)
             .ToList();

        if (categories.Count > 0)
        {
            foreach (var model in categories)
            {
                var category = new Category
                {
                    Id = model.Id,
                    Name = model.Name,
                    Image = model.Image
                };
                var result = await DeleteAsync(category);
                if (result)
                {
                    AllCategories.Remove(model);
                    Categories.Remove(model);
                }
            }

            Categories.ForEach(x => x.IsSelected = false);
            IsAllSelected = false;
            ErrorMessage = null;
        }
        IsBusy = false;
    }

    private async Task<bool> DeleteAsync(Category category)
    {
        var result = await CategoryService.DeleteCategoryByIdAsync(category.Id);
        if (result == false)
        {
            ErrorMessage = AppServices.Error ?? Localizer[nameof(Translate.UnknowError)].Value;
            await Task.Delay(2000);
        }
        else
        {
            await FileService.DeleteFileAsync(category.Image, "images", "collection");
            return true;
        }
        return false;
    }

    private void OnSelectAllChanged(ChangeEventArgs args)
    {
        if (args.Value is not bool value)
        {
            return;
        }

        IsAllSelected = value;
        Categories.ForEach(x => x.IsSelected = value);
    }

    private void OnSelectChanged(ChangeEventArgs args)
    {
        if (args.Value is not bool value)
        {
            return;
        }

        IsAllSelected = Categories.Where(x => x.IsSelected).Count() == Categories.Count;
    }
}
