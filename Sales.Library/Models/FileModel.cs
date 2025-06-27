namespace Sales.Library.Models;

public class FileModel
{
    public byte[] Bytes { get; set; } = [];
    public required string FileName { get; set; } = string.Empty;
    public required string Directory { get; set; } = string.Empty;
    public string? SubDirectory { get; set; }
    public string? NewFileName { get; set; }
}
