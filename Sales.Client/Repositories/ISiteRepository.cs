
namespace Sales.Client.Repositories
{
    public interface ISiteRepository
    {
        Task<int> GetItemPerPageAsync();
    }
}
