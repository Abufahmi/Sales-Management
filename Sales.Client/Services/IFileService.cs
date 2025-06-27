
using Microsoft.AspNetCore.Components.Forms;
using Sales.Library.Models;

namespace Sales.Client.Services;

public interface IFileService
{
    Task<bool> DeleteFileAsync(string xFile, string directory, string? subDirectory);
    Task<byte[]> GetFileBrowserBytesAsync(IBrowserFile browserFile);
    Task<bool> IsFileExistsAsync(string name, string directory, string? subDirectory);
    Task<bool> UploadFileAsync(FileModel fileModel);
}
