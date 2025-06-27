using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sales.Library.Models;

namespace Sales.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileUploadsController(IWebHostEnvironment host) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("IsFileExists")]
    public IActionResult IsFileExists(string name, string directory, string? subDirectory = null)
    {
        var dir = Path.Combine(host.WebRootPath, directory);
        if (subDirectory is not null && !string.IsNullOrEmpty(subDirectory))
        {
            dir = Path.Combine(dir, subDirectory);
        }

        var filePath = Path.Combine(dir, name);
        if (System.IO.File.Exists(filePath))
        {
            return Ok();
        }
        return NotFound();
    }

    [Authorize]
    [HttpPost]
    [Route("UploadFile")]
    public async Task<IActionResult> UploadFile(FileModel fileModel)
    {
        if (fileModel.Bytes.Length < 1) return BadRequest("File is empty or not provided.");
        if (string.IsNullOrEmpty(fileModel.FileName))
        {
            return BadRequest("File name is required.");
        }

        var dir = Path.Combine(host.WebRootPath, fileModel.Directory);
        if (fileModel.SubDirectory is not null && !string.IsNullOrEmpty(fileModel.SubDirectory))
        {
            dir = Path.Combine(dir, fileModel.SubDirectory);
        }
        if (!Directory.Exists(dir))
        {
            return BadRequest("Directory does not exist.");
        }

        try
        {
            var filePath = Path.Combine(dir, fileModel.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await stream.WriteAsync(fileModel.Bytes.AsMemory(0, fileModel.Bytes.Length));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"An unknown error occurred: {ex.Message}");
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("DeleteFile")]
    public IActionResult DeleteFile(string name, string directory, string? subDirectory = null)
    {
        var dir = Path.Combine(host.WebRootPath, directory);
        if (subDirectory is not null && !string.IsNullOrEmpty(subDirectory))
        {
            dir = Path.Combine(dir, subDirectory);
        }

        var filePath = Path.Combine(dir, name);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File does not exist.");
        }
        try
        {
            System.IO.File.Delete(filePath);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"An unknown error occurred: {ex.Message}");
        }
    }
}
