namespace Sales.Library.Services;

public class ResponseService<T>
{
    public required bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public List<T>? TList { get; set; }
    public T? TItem { get; set; }
}
