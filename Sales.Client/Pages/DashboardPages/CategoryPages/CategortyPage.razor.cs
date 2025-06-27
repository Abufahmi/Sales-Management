using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Sales.Client.Helpers;
using Sales.Client.Models;
using Sales.Client.Services;
using Sales.Library.Contracts;
using Sales.Library.Entities;
using Sales.Library.Models;
using Translate = Sales.Client.Resources.App;

namespace Sales.Client.Pages.DashboardPages.CategoryPages;

public partial class CategortyPage
{
    #region Properties

    [Inject] ICategoryService CategoryService { get; set; } = default!;
    [Inject] IFileService FileService { get; set; } = default!;

    [Parameter] public int Id { get; set; }
    CategoryModel Model { get; set; } = new();
    List<Category> Categories { get; set; } = [];
    bool IsBusy { get; set; }
    string Title { get; set; } = string.Empty;
    string? ErrorMessage { get; set; }
    string? SuccessMessage { get; set; }
    string? NameError { get; set; }
    IBrowserFile? BrowserFile { get; set; }
    string? FileError { get; set; }
    string XFile { get; set; } = string.Empty;
    int MaxFileSize { get; set; } = 5 * 1024 * 1024; // 5 MB

    #endregion

    protected override async Task OnInitializedAsync()
    {
        var categories = await CategoryService.GetCategoryListAsync();
        if (categories != null)
        {
            Categories = categories;
        }

        if (Id > 0)
        {
            var category = Categories.FirstOrDefault(c => c.Id == Id);
            if (category != null)
            {
                Model = ModelConverter.GetCategoryModel(category);
                XFile = Model.Image;
                Title = Localizer[nameof(Translate.EditCategory)].Value;
            }
            else
            {
                NavManager.NavigateTo("not-found");
            }
        }
        else
        {
            Title = Localizer[nameof(Translate.AddCategory)].Value;
        }
    }

    private async Task OnValidSubmitAsync()
    {
        if (FileError is not null || NameError is not null)
        {
            return;
        }

        IsBusy = true;
        var category = new Category
        {
            Name = Model.Name,
            Image = Model.Image
        };

        if (Id > 0)
        {
            category.Id = Id;
            await UpdateAsync(category);
        }
        else
        {
            await CreateAsync(category);
        }

        IsBusy = false;
    }

    private async Task CreateAsync(Category category)
    {
        if (BrowserFile is null)
        {
            return;
        }

        var bytes = await FileService.GetFileBrowserBytesAsync(BrowserFile);
        var fileModel = new FileModel
        {
            Bytes = bytes,
            Directory = "images",
            SubDirectory = "collection",
            FileName = BrowserFile.Name,
            NewFileName = null
        };

        bool uploaded = await FileService.UploadFileAsync(fileModel);
        if (!uploaded)
        {
            ErrorMessage = AppServices.Error ?? Localizer[nameof(Translate.ErrorUploadingFile)].Value;
            return;
        }

        var result = await CategoryService.CreateCategoryAsync(category);
        if (result != null)
        {
            SuccessMessage = Localizer[nameof(Translate.CreatedSuccessfully)].Value;
            Model = new();
            Categories.Add(result);
            BrowserFile = null;
        }
        else
        {
            ErrorMessage = AppServices.Error ?? Localizer[nameof(Translate.UnknowError)].Value;
        }
    }

    private async Task UpdateAsync(Category category)
    {
        if (BrowserFile is not null)
        {
            var bytes = await FileService.GetFileBrowserBytesAsync(BrowserFile);
            var fileModel = new FileModel
            {
                Bytes = bytes,
                Directory = "images",
                SubDirectory = "collection",
                FileName = BrowserFile.Name,
                NewFileName = null
            };

            bool uploaded = await FileService.UploadFileAsync(fileModel);
            if (!uploaded)
            {
                ErrorMessage = AppServices.Error ?? Localizer[nameof(Translate.ErrorUploadingFile)].Value;
                return;
            }
        }

        var result = await CategoryService.UpdateCategoryAsync(category);
        if (result)
        {
            Model = new();
            HideMessage();
            if (BrowserFile is not null)
            {
                await FileService.DeleteFileAsync(XFile, "images", "collection");
            }

            NavManager.NavigateTo("categories");
        }
        else
        {
            ErrorMessage = AppServices.Error ?? Localizer[nameof(Translate.UnknowError)].Value;
        }
    }

    private void HideMessage()
    {
        ErrorMessage = null;
        SuccessMessage = null;
    }

    private void OnNameChange(ChangeEventArgs args)
    {
        NameError = null;
        if (args?.Value is not string value)
        {
            return;
        }
        else
        {
            bool result;
            if (Id > 0)
            {
                result = Categories.Any(c => c.Name == value && c.Id != Id);
            }
            else
            {
                result = Categories.Any(c => c.Name == value);
            }

            if (result)
            {
                NameError = Localizer[nameof(Translate.CategoryNameAlreadyExists)].Value;
            }
        }
    }

    private async Task OnLoadFileAsync(InputFileChangeEventArgs args)
    {
        if (args.File.Size > MaxFileSize)
        {
            FileError = string.Format(Localizer[nameof(Translate.FileUploadExceeded)].Value,
                "5");
            return;
        }

        Model.Image = args.File.Name;
        BrowserFile = args.File;
        var stream = args.File.OpenReadStream(MaxFileSize);
        var dotNet = new DotNetStreamReference(stream);
        await Js.InvokeVoidAsync("setImageUsingStreaming", "image", dotNet);
        await CheckFileExistsAsync(args.File.Name);
    }

    private async Task CheckFileExistsAsync(string name)
    {
        FileError = null;
        bool exists = await FileService.IsFileExistsAsync(name, "images", "collection");
        if (exists)
        {
            FileError = Localizer[nameof(Translate.FileExists)].Value;
        }
    }

    private async Task OnCancel(EventArgs args)
    {
        BrowserFile = null;
        if (Id > 0)
        {
            Model.Image = XFile;
        }
        else
        {
            Model.Image = string.Empty;
        }
        await Task.CompletedTask;
    }
}
