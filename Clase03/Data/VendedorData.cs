using System.Data;
using Microsoft.Data.SqlClient;
using Clase03.Models;

namespace Clase03.Data
{
    public class VendedorData
    {
        private readonly SqlData _sql;

        public VendedorData(SqlData sql)
        {
            _sql = sql;
        }

        // LISTAR TODO usando Stored Procedure
        public List<Vendedor> Listar()
        {
            DataTable dt = _sql.ExecuteDataTable("sp_Vendedor_Listar", CommandType.StoredProcedure);

            List<Vendedor> lista = new List<Vendedor>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Vendedor
                {
                    VendedorId = (int)row["VendedorId"],
                    Nombre = row["Nombre"].ToString(),
                    EsActivo = (bool)row["EsActivo"]
                });
            }

            return lista;
        }


        // INSERTAR
        public int Insertar(string nombre)
        {
            return _sql.ExecuteNonQuery(
                "sp_Vendedor_Insert",
                new SqlParameter("@Nombre", nombre)
            );
        }

        // ACTUALIZAR
        public int Actualizar(Vendedor v)
        {
            return _sql.ExecuteNonQuery(
                "sp_Vendedor_Update",
                new SqlParameter("@VendedorId", v.VendedorId),
                new SqlParameter("@Nombre", v.Nombre),
                new SqlParameter("@EsActivo", v.EsActivo)
            );
        }

        // ELIMINAR (LÓGICO)
        public int Eliminar(int id)
        {
            return _sql.ExecuteNonQuery(
                "sp_Vendedor_Delete",
                new SqlParameter("@VendedorId", id)
            );
        }
    }
}
