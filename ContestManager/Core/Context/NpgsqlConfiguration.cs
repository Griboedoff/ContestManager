using System.Data.Entity;
using Npgsql;

namespace Core.Context
{
    public class NpgsqlConfiguration : DbConfiguration
    {
        public NpgsqlConfiguration()
        {
            SetProviderServices("Npgsql", NpgsqlServices.Instance);
            SetProviderFactory("Npgsql", NpgsqlFactory.Instance);
        }
    }
}