using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetAPI.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T? LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingleOrDefault<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            SqlCommand commandWithParams = new SqlCommand(sql);

            foreach (SqlParameter parameter in parameters)
            {
                commandWithParams.Parameters.Add(parameter);
            }

            dbConnection.Open();

            commandWithParams.Connection = dbConnection;

            int rowsAffected = commandWithParams.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}