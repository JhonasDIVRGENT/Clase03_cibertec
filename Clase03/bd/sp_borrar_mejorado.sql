CREATE OR ALTER PROCEDURE sp_Vendedor_Delete
    @VendedorId INT
AS
BEGIN
    UPDATE Vendedores
    SET EsActivo = 0
    WHERE VendedorId = @VendedorId;
END
GO
