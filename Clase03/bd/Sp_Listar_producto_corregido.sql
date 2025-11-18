CREATE OR ALTER PROCEDURE sp_Producto_Listar
AS
BEGIN
    SELECT 
        p.ProductoId,
        p.Codigo,
        p.Nombre,
        p.Categoria,
        p.Precio,
        p.Stock,
        p.VendedorId,
        p.EsActivo,
        v.Nombre AS VendedorNombre
    FROM Productos p
    LEFT JOIN Vendedores v ON p.VendedorId = v.VendedorId
    WHERE p.EsActivo = 1;
END
GO
