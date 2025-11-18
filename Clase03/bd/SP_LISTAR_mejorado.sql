CREATE OR ALTER PROCEDURE sp_Vendedor_Listar
AS
BEGIN
    SELECT VendedorId, Nombre, EsActivo
    FROM Vendedores
    WHERE EsActivo = 1
END
GO
