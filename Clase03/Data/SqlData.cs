using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Clase03.Data
{
    public class SqlData
    {
        private readonly string _connectionString;

        public SqlData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' no está configurada.");
        }

        // Método flexible para ejecutar SP o consultas SQL directas
        public DataTable ExecuteDataTable(string commandText, CommandType commandType = CommandType.StoredProcedure, params SqlParameter[] parameters)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(commandText, cn);
            cmd.CommandType = commandType;

            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(storedProcedure, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            cn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}
