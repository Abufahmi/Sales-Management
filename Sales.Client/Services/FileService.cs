
using Microsoft.AspNetCore.Components.Forms;
using Sales.Client.Helpers;
using Sales.Library.Models;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace Sales.Client.Services;

public class FileService(ClientService clientService) : IFileService
{
    private readonly long maxFileSize = 5 * 1024 * 1024; // 5 MB

    public async Task<bool> DeleteFileAsync(string xFile, string directory, string? subDirectory)
    {
        AppServices.Error = null;
        var url = $"{AppServices.BaseApiAddress}FileUploads/DeleteFile?name={xFile}" +
            $"&directory={directory}&subDirectory={subDirectory}";
        try
        {
            var client = await clientService.GetAuthorizeClientAsync();
            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode == false)
            {
                AppServices.Error = await response.Content.ReadAsStringAsync();
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            AppServices.Error = ex.Message;
        }
        return false;
    }

    public async Task<byte[]> GetFileBrowserBytesAsync(IBrowserFile browserFile)
    {
        using MemoryStream memoryStream = new();
        await browserFile.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public async Task<bool> IsFileExistsAsync(string name, string directory, string? subDirectory)
    {
        AppServices.Error = null;
        var url = $"{AppServices.BaseApiAddress}FileUploads/IsFileExists?name={name}" +
            $"&directory={directory}&subDirectory={subDirectory}";
        try
        {
            var client = await clientService.GetAuthorizeClientAsync();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                AppServices.Error = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            AppServices.Error = ex.Message;
        }
        return false;
    }

    public async Task<bool> UploadFileAsync(FileModel fileModel)
    {
        AppServices.Error = null;
        try
        {
            var client = await clientService.GetAuthorizeClientAsync();
            var url = $"{AppServices.BaseApiAddress}FileUploads/UploadFile";
            var response = await client.PostAsJsonAsync(url, fileModel);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                AppServices.Error = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            AppServices.Error = ex.Message;
        }
        return false;
    }
}
