using System.Data;
using System.Data.SqlClient;

namespace Gerenciamento.Context
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString =  _configuration.GetConnectionString("DefaultConnection");          
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
