using System.Data;
using Microsoft.Data.SqlClient;
using Clase03.Models;

namespace Clase03.Data
{
    public class ProductoData
    {
        private readonly SqlData _sql;

        public ProductoData(SqlData sql)
        {
            _sql = sql;
        }

        // LISTAR TODO usando Stored Procedure sp_Producto_Listar
        //listar filtrado

        public List<Producto> ListarFiltrado(string terminoBusqueda)
        {
            var parametros = new SqlParameter[]
            {
        new SqlParameter("@TerminoBusqueda", (object)terminoBusqueda ?? DBNull.Value)
            };

            DataTable dt = _sql.ExecuteDataTable(
                "sp_Producto_Listar_Filtrado",
                CommandType.StoredProcedure,
                parametros
            );

            var lista = new List<Producto>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Producto
                {
                    ProductoId = (int)row["ProductoId"],
                    Codigo = row["Codigo"].ToString(),
                    Nombre = row["Nombre"].ToString(),
                    Categoria = row["Categoria"]?.ToString(),
                    Precio = (decimal)row["Precio"],
                    Stock = (int)row["Stock"],
                    VendedorId = row["VendedorId"] != DBNull.Value ? (int?)row["VendedorId"] : null,
                    EsActivo = (bool)row["EsActivo"],
                    VendedorNombre = row["VendedorNombre"]?.ToString()
                });
            }

            return lista;
        }

        //lISTAR
        public List<Producto> Listar()
        {
            // Llamamos al SP que ya tienes creado para listar productos
            DataTable dt = _sql.ExecuteDataTable("sp_Producto_Listar", CommandType.StoredProcedure);

            var lista = new List<Producto>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Producto
                {
                    ProductoId = (int)row["ProductoId"],
                    Codigo = row["Codigo"].ToString(),
                    Nombre = row["Nombre"].ToString(),
                    Categoria = row["Categoria"]?.ToString(),
                    Precio = (decimal)row["Precio"],
                    Stock = (int)row["Stock"],
                    VendedorId = row["VendedorId"] != DBNull.Value ? (int?)row["VendedorId"] : null,
                    EsActivo = (bool)row["EsActivo"],
                    VendedorNombre = row["VendedorNombre"]?.ToString()
                });
            }

            return lista;
        }

        // INSERTAR
        public int Insertar(Producto p)
        {
            return _sql.ExecuteNonQuery(
                "sp_Producto_Insert",
                new SqlParameter("@Codigo", p.Codigo),
                new SqlParameter("@Nombre", p.Nombre),
                new SqlParameter("@Categoria", (object?)p.Categoria ?? DBNull.Value),
                new SqlParameter("@Precio", p.Precio),
                new SqlParameter("@Stock", p.Stock),
                new SqlParameter("@VendedorId", (object?)p.VendedorId ?? DBNull.Value)
            );
        }

        // ACTUALIZAR
        public int Actualizar(Producto p)
        {
            return _sql.ExecuteNonQuery(
                "sp_Producto_Update",
                new SqlParameter("@ProductoId", p.ProductoId),
                new SqlParameter("@Codigo", p.Codigo),
                new SqlParameter("@Nombre", p.Nombre),
                new SqlParameter("@Categoria", (object?)p.Categoria ?? DBNull.Value),
                new SqlParameter("@Precio", p.Precio),
                new SqlParameter("@Stock", p.Stock),
                new SqlParameter("@VendedorId", (object?)p.VendedorId ?? DBNull.Value),
                new SqlParameter("@EsActivo", p.EsActivo)
            );
        }

        // DELETE lógico
        public int Eliminar(int id)
        {
            return _sql.ExecuteNonQuery(
                "sp_Producto_Delete",
                new SqlParameter("@ProductoId", id)
            );
        }
    }
}
