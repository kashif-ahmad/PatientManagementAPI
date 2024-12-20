using System.Data;
using System.Data.SqlClient;

namespace PatientManagementApi.Util
{
    public class DbConnectionHelper
    {
        private readonly string _connectionString;

        public DbConnectionHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}