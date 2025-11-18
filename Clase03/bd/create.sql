------------------------------------------------------------
-- 1) CREAR BASE DE DATOS
------------------------------------------------------------
IF DB_ID('Clase03DB') IS NOT NULL
    DROP DATABASE Clase03DB;
GO

CREATE DATABASE Clase03DB;
GO

USE Clase03DB;
GO


------------------------------------------------------------
-- 2) TABLA: Vendedores
------------------------------------------------------------
CREATE TABLE Vendedores (
    VendedorId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    EsActivo BIT NOT NULL DEFAULT 1
);
GO


------------------------------------------------------------
-- 3) TABLA: Productos
------------------------------------------------------------
CREATE TABLE Productos (
    ProductoId INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(50) NOT NULL,
    Nombre NVARCHAR(250) NOT NULL,
    Categoria NVARCHAR(150) NULL,
    Precio DECIMAL(10,2) NOT NULL DEFAULT 0,
    Stock INT NOT NULL DEFAULT 0,
    VendedorId INT NULL,
    EsActivo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Productos_Vendedores FOREIGN KEY (VendedorId)
        REFERENCES Vendedores(VendedorId)
);
GO


------------------------------------------------------------
-- 4) INSERTS DE PRUEBA
------------------------------------------------------------
INSERT INTO Vendedores (Nombre) VALUES
 (N'Juan Pérez'),
 (N'Ana Torres'),
 (N'Marco Ruiz'),
 (N'Lucía Morales'),
 (N'Sofía Ávila');
GO

INSERT INTO Productos (Codigo, Nombre, Categoria, Precio, Stock, VendedorId)
VALUES
 ('P001', 'Laptop Lenovo', 'Tecnología', 2500, 10, 1),
 ('P002', 'Mouse Logitech', 'Accesorios', 120, 40, 1),
 ('P003', 'Teclado Redragon', 'Accesorios', 160, 20, 2),
 ('P004', 'Monitor Samsung 27"', 'Tecnología', 900, 15, 3),
 ('P005', 'Audífonos Sony', 'Audio', 300, 25, 4),
 ('P006', 'Parlante JBL', 'Audio', 350, 35, 5),
 ('P007', 'Cable HDMI', 'Accesorios', 30, 100, 2),
 ('P008', 'iPad Mini', 'Tecnología', 1500, 5, 3),
 ('P009', 'SSD Kingston 500GB', 'Tecnología', 260, 18, 4),
 ('P010', 'Tarjeta de Video RTX 4060', 'Tecnología', 1800, 4, 1);
GO


------------------------------------------------------------
-- 5) SP CRUD: Vendedores
------------------------------------------------------------

-- INSERT
CREATE OR ALTER PROCEDURE sp_Vendedor_Insert
    @Nombre NVARCHAR(200)
AS
BEGIN
    INSERT INTO Vendedores (Nombre)
    VALUES (@Nombre);
END
GO

-- UPDATE
CREATE OR ALTER PROCEDURE sp_Vendedor_Update
    @VendedorId INT,
    @Nombre NVARCHAR(200),
    @EsActivo BIT
AS
BEGIN
    UPDATE Vendedores
    SET Nombre = @Nombre,
        EsActivo = @EsActivo
    WHERE VendedorId = @VendedorId;
END
GO

-- DELETE (LÓGICO)
CREATE OR ALTER PROCEDURE sp_Vendedor_Delete
    @VendedorId INT
AS
BEGIN
    UPDATE Vendedores
    SET EsActivo = 0
    WHERE VendedorId = @VendedorId;
END
GO


------------------------------------------------------------
-- 6) SP FILTRAR/PAGINAR Vendedores
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_Vendedores_GetPaged
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @Search NVARCHAR(250) = NULL,
    @SortBy NVARCHAR(50) = N'VeendedorId',
    @SortDir NVARCHAR(4) = N'ASC',
    @TotalRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 10;
    IF @SortDir NOT IN ('ASC','DESC') SET @SortDir = 'ASC';

    DECLARE @Where NVARCHAR(MAX) = N' WHERE v.EsActivo = 1 ';

    IF (@Search IS NOT NULL AND LEN(@Search) > 0)
    BEGIN
        SET @Search = '%' + REPLACE(@Search, ' ', '%') + '%';
        SET @Where = @Where + N' AND v.Nombre LIKE @pSearch ';
    END

    DECLARE @SqlCount NVARCHAR(MAX) =
    N'SELECT @out = COUNT(1)
      FROM Vendedores v ' + @Where;

    EXEC sp_executesql
        @SqlCount,
        N'@pSearch NVARCHAR(250), @out INT OUTPUT',
        @pSearch = @Search,
        @out = @TotalRows OUTPUT;

    DECLARE @Order NVARCHAR(200) = QUOTENAME(@SortBy) + ' ' + @SortDir;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    DECLARE @SqlPaged NVARCHAR(MAX) =
    N'SELECT 
        v.VendedorId,
        v.Nombre,
        v.EsActivo
      FROM Vendedores v
      ' + @Where + '
      ORDER BY ' + @Order + '
      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;';

    EXEC sp_executesql
        @SqlPaged,
        N'@pSearch NVARCHAR(250), @Offset INT, @PageSize INT',
        @pSearch = @Search, @Offset = @Offset, @PageSize = @PageSize;
END
GO



------------------------------------------------------------
-- 7) SP CRUD: Productos
------------------------------------------------------------

-- INSERT
CREATE OR ALTER PROCEDURE sp_Producto_Insert
    @Codigo NVARCHAR(50),
    @Nombre NVARCHAR(250),
    @Categoria NVARCHAR(150),
    @Precio DECIMAL(10,2),
    @Stock INT,
    @VendedorId INT NULL
AS
BEGIN
    INSERT INTO Productos (Codigo, Nombre, Categoria, Precio, Stock, VendedorId)
    VALUES (@Codigo, @Nombre, @Categoria, @Precio, @Stock, @VendedorId);
END
GO

-- UPDATE
CREATE OR ALTER PROCEDURE sp_Producto_Update
    @ProductoId INT,
    @Codigo NVARCHAR(50),
    @Nombre NVARCHAR(250),
    @Categoria NVARCHAR(150),
    @Precio DECIMAL(10,2),
    @Stock INT,
    @VendedorId INT NULL,
    @EsActivo BIT
AS
BEGIN
    UPDATE Productos
    SET Codigo = @Codigo,
        Nombre = @Nombre,
        Categoria = @Categoria,
        Precio = @Precio,
        Stock = @Stock,
        VendedorId = @VendedorId,
        EsActivo = @EsActivo
    WHERE ProductoId = @ProductoId;
END
GO

-- DELETE (LÓGICO)
CREATE OR ALTER PROCEDURE sp_Producto_Delete
    @ProductoId INT
AS
BEGIN
    UPDATE Productos
    SET EsActivo = 0
    WHERE ProductoId = @ProductoId;
END
GO


------------------------------------------------------------
-- 8) SP FILTRAR/PAGINAR Productos
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_Productos_GetPaged
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @Search NVARCHAR(250) = NULL,
    @SortBy NVARCHAR(50) = N'ProductoId',
    @SortDir NVARCHAR(4) = N'ASC',
    @TotalRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 10;
    IF @SortDir NOT IN ('ASC','DESC') SET @SortDir = 'ASC';

    DECLARE @Where NVARCHAR(MAX) = N' WHERE p.EsActivo = 1 ';

    IF (@Search IS NOT NULL AND LEN(@Search) > 0)
    BEGIN
        SET @Search = '%' + REPLACE(@Search, ' ', '%') + '%';
        SET @Where = @Where + N'
            AND (p.Codigo LIKE @pSearch 
             OR p.Nombre LIKE @pSearch
             OR p.Categoria LIKE @pSearch)';
    END

    DECLARE @SqlCount NVARCHAR(MAX) =
    N'SELECT @out = COUNT(1)
      FROM Productos p
      ' + @Where;

    EXEC sp_executesql
        @SqlCount,
        N'@pSearch NVARCHAR(250), @out INT OUTPUT',
        @pSearch = @Search,
        @out = @TotalRows OUTPUT;

    DECLARE @Order NVARCHAR(200) = QUOTENAME(@SortBy) + ' ' + @SortDir;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    DECLARE @SqlPaged NVARCHAR(MAX) = 
    N'SELECT 
        p.ProductoId,
        p.Codigo,
        p.Nombre,
        p.Categoria,
        p.Precio,
        p.Stock,
        p.EsActivo,
        v.Nombre AS VendedorNombre
      FROM Productos p
      LEFT JOIN Vendedores v ON p.VendedorId = v.VendedorId
      ' + @Where + '
      ORDER BY ' + @Order + '
      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;';

    EXEC sp_executesql
        @SqlPaged,
        N'@pSearch NVARCHAR(250), @Offset INT, @PageSize INT',
        @pSearch = @Search, @Offset = @Offset, @PageSize = @PageSize;
END
GO

------------------------------------------------------------
-- FIN DEL SCRIPT COMPLETO
------------------------------------------------------------
