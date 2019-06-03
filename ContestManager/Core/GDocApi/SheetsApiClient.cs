using System.Threading.Tasks;

namespace Core.GDocApi
{
    public interface ISheetsApiClient
    {
        Task<string> CreateTable();
    }
}