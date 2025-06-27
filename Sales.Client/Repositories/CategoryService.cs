using Sales.Client.Helpers;
using Sales.Client.Services;
using Sales.Library.Contracts;
using Sales.Library.Entities;
using Sales.Library.Models;
using Sales.Library.Services;
using System.Net.Http.Json;

namespace Sales.Client.Repositories;

public class CategoryService(ClientService clientService) : ICategoryService
{
    public async Task<Category?> CreateCategoryAsync(Category category)
    {
        AppServices.Error = null;
        var httpClient = await clientService.GetAuthorizeClientAsync();
        var url = "Categories/CreateCategory";
        HttpResponseMessage result = await httpClient.PostAsJsonAsync(url, category);
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<Category>();
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return null;
    }

    public async Task<bool> DeleteCategoryByIdAsync(int id)
    {
        AppServices.Error = null;
        var httpClient = await clientService.GetAuthorizeClientAsync();
        var url = $"Categories/DeleteCategoryById/{id}";
        HttpResponseMessage result = await httpClient.DeleteAsync(url);
        if (result.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return false;
    }

    public async Task<bool> DeleteCategoryListAsync(List<int> ids)
    {
        AppServices.Error = null;
        var httpClient = await clientService.GetAuthorizeClientAsync();
        var serialize = SerializationService<List<int>>.SerializeModel(ids);
        var url = $"Categories/DeleteCategoryList/{serialize}";
        HttpResponseMessage result = await httpClient.DeleteAsync(url);
        if (result.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return false;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        AppServices.Error = null;
        var httpClient = clientService.GetClient();
        var url = $"Categories/GetCategoryById/{id}";
        HttpResponseMessage result = await httpClient.GetAsync(url);
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<Category>();
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return null;
    }

    public async Task<List<Category>?> GetCategoryListAsync()
    {
        AppServices.Error = null;
        var httpClient = clientService.GetClient();
        var url = $"Categories/GetCategoryList";
        HttpResponseMessage result = await httpClient.GetAsync(url);
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<List<Category>>();
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return null;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        AppServices.Error = null;
        var httpClient = await clientService.GetAuthorizeClientAsync();
        var url = "Categories/UpdateCategory";
        HttpResponseMessage result = await httpClient.PutAsJsonAsync(url, category);
        if (result.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var error = await result.Content.ReadAsStringAsync();
            if (error != null && !string.IsNullOrEmpty(error))
                AppServices.Error = error;
        }
        return false;
    }
}
