namespace Clase03.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int? VendedorId { get; set; }
        public bool EsActivo { get; set; }

        public string? VendedorNombre { get; set; } // Para mostrar en vistas
    }
}
