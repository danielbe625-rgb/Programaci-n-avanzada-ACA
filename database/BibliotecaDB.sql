/* =====================================================================
   Sistema de Gestión de Biblioteca - Programación Avanzada
   Script SQL Server: creación de BD, tablas, restricciones, datos de
   prueba y vista para reportes.
   ===================================================================== */

USE master;
GO

IF DB_ID(N'BibliotecaDB') IS NOT NULL
BEGIN
    ALTER DATABASE BibliotecaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BibliotecaDB;
END
GO

CREATE DATABASE BibliotecaDB;
GO

USE BibliotecaDB;
GO

/* ---------------------------------------------------------------------
   TABLA: Autores
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Autores
(
    IdAutor   INT IDENTITY(1,1) NOT NULL,
    Nombre    NVARCHAR(60)      NOT NULL,
    Apellido  NVARCHAR(60)      NOT NULL,
    CONSTRAINT PK_Autores PRIMARY KEY (IdAutor),
    CONSTRAINT UQ_Autores_NombreApellido UNIQUE (Nombre, Apellido)
);
GO

/* ---------------------------------------------------------------------
   TABLA: Editoriales
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Editoriales
(
    IdEditorial INT IDENTITY(1,1) NOT NULL,
    Nombre      NVARCHAR(100)     NOT NULL,
    Pais        NVARCHAR(60)      NULL,
    CONSTRAINT PK_Editoriales PRIMARY KEY (IdEditorial),
    CONSTRAINT UQ_Editoriales_Nombre UNIQUE (Nombre)
);
GO

/* ---------------------------------------------------------------------
   TABLA: Usuarios
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Usuarios
(
    IdUsuario INT IDENTITY(1,1) NOT NULL,
    Nombre    NVARCHAR(60)      NOT NULL,
    Apellido  NVARCHAR(60)      NOT NULL,
    Documento NVARCHAR(20)      NOT NULL,
    Telefono  NVARCHAR(20)      NULL,
    Correo    NVARCHAR(100)     NULL,
    CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuarios_Documento UNIQUE (Documento),
    CONSTRAINT CK_Usuarios_Correo CHECK (Correo IS NULL OR Correo LIKE '%_@_%._%')
);
GO

/* ---------------------------------------------------------------------
   TABLA: Libros
   Existencias = ejemplares disponibles actualmente para préstamo.
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Libros
(
    IdLibro         INT IDENTITY(1,1) NOT NULL,
    Codigo          NVARCHAR(20)      NOT NULL,   -- Código interno o ISBN
    Titulo          NVARCHAR(150)     NOT NULL,
    IdAutor         INT               NOT NULL,
    IdEditorial     INT               NOT NULL,
    AnioPublicacion INT               NULL,
    Existencias     INT               NOT NULL CONSTRAINT DF_Libros_Existencias DEFAULT (1),
    CONSTRAINT PK_Libros PRIMARY KEY (IdLibro),
    CONSTRAINT UQ_Libros_Codigo UNIQUE (Codigo),
    CONSTRAINT FK_Libros_Autores FOREIGN KEY (IdAutor)
        REFERENCES dbo.Autores (IdAutor),
    CONSTRAINT FK_Libros_Editoriales FOREIGN KEY (IdEditorial)
        REFERENCES dbo.Editoriales (IdEditorial),
    CONSTRAINT CK_Libros_Existencias CHECK (Existencias >= 0),
    CONSTRAINT CK_Libros_Anio CHECK (AnioPublicacion IS NULL OR AnioPublicacion BETWEEN 1000 AND 2100)
);
GO

/* ---------------------------------------------------------------------
   TABLA: Prestamos
   Estado: 'Prestado' (sin fecha de devolución) o 'Devuelto' (con fecha).
   --------------------------------------------------------------------- */
CREATE TABLE dbo.Prestamos
(
    IdPrestamo       INT IDENTITY(1,1) NOT NULL,
    IdUsuario        INT               NOT NULL,
    IdLibro          INT               NOT NULL,
    FechaPrestamo    DATE              NOT NULL CONSTRAINT DF_Prestamos_Fecha DEFAULT (CAST(GETDATE() AS DATE)),
    FechaDevolucion  DATE              NULL,
    Estado           NVARCHAR(10)      NOT NULL CONSTRAINT DF_Prestamos_Estado DEFAULT (N'Prestado'),
    CONSTRAINT PK_Prestamos PRIMARY KEY (IdPrestamo),
    CONSTRAINT FK_Prestamos_Usuarios FOREIGN KEY (IdUsuario)
        REFERENCES dbo.Usuarios (IdUsuario),
    CONSTRAINT FK_Prestamos_Libros FOREIGN KEY (IdLibro)
        REFERENCES dbo.Libros (IdLibro),
    CONSTRAINT CK_Prestamos_Estado CHECK (Estado IN (N'Prestado', N'Devuelto')),
    CONSTRAINT CK_Prestamos_Fechas CHECK (FechaDevolucion IS NULL OR FechaDevolucion >= FechaPrestamo),
    CONSTRAINT CK_Prestamos_Coherencia CHECK (
        (Estado = N'Prestado' AND FechaDevolucion IS NULL) OR
        (Estado = N'Devuelto' AND FechaDevolucion IS NOT NULL))
);
GO

CREATE INDEX IX_Prestamos_Usuario ON dbo.Prestamos (IdUsuario);
CREATE INDEX IX_Prestamos_Libro   ON dbo.Prestamos (IdLibro);
CREATE INDEX IX_Libros_Titulo     ON dbo.Libros (Titulo);
GO

/* ---------------------------------------------------------------------
   VISTA para reportes: detalle de préstamos
   --------------------------------------------------------------------- */
CREATE VIEW dbo.vw_PrestamosDetalle
AS
SELECT  p.IdPrestamo,
        u.Documento,
        u.Nombre + N' ' + u.Apellido AS Usuario,
        l.Codigo,
        l.Titulo,
        a.Nombre + N' ' + a.Apellido AS Autor,
        e.Nombre                     AS Editorial,
        p.FechaPrestamo,
        p.FechaDevolucion,
        p.Estado
FROM    dbo.Prestamos   p
JOIN    dbo.Usuarios    u ON u.IdUsuario   = p.IdUsuario
JOIN    dbo.Libros      l ON l.IdLibro     = p.IdLibro
JOIN    dbo.Autores     a ON a.IdAutor     = l.IdAutor
JOIN    dbo.Editoriales e ON e.IdEditorial = l.IdEditorial;
GO

/* ---------------------------------------------------------------------
   DATOS DE PRUEBA
   --------------------------------------------------------------------- */
INSERT INTO dbo.Autores (Nombre, Apellido) VALUES
(N'Gabriel',  N'García Márquez'),
(N'Isabel',   N'Allende'),
(N'Julio',    N'Cortázar'),
(N'Mario',    N'Vargas Llosa'),
(N'Jorge Luis', N'Borges');

INSERT INTO dbo.Editoriales (Nombre, Pais) VALUES
(N'Sudamericana', N'Argentina'),
(N'Alfaguara',    N'España'),
(N'Planeta',      N'España'),
(N'Penguin Random House', N'Estados Unidos');

INSERT INTO dbo.Usuarios (Nombre, Apellido, Documento, Telefono, Correo) VALUES
(N'Laura',  N'Gómez',    N'1010101010', N'3001112233', N'laura.gomez@correo.com'),
(N'Carlos', N'Pérez',    N'2020202020', N'3104445566', N'carlos.perez@correo.com'),
(N'Marta',  N'Rodríguez',N'3030303030', N'3207778899', NULL),
(N'Andrés', N'Torres',   N'4040404040', NULL,           N'andres.torres@correo.com');

INSERT INTO dbo.Libros (Codigo, Titulo, IdAutor, IdEditorial, AnioPublicacion, Existencias) VALUES
(N'978-0307474728', N'Cien años de soledad',        1, 1, 1967, 3),
(N'978-0307387264', N'El amor en los tiempos del cólera', 1, 2, 1985, 2),
(N'978-0525433477', N'La casa de los espíritus',    2, 2, 1982, 2),
(N'978-8437604947', N'Rayuela',                     3, 1, 1963, 1),
(N'978-8420471839', N'La ciudad y los perros',      4, 2, 1963, 2),
(N'978-8420633123', N'Ficciones',                   5, 3, 1944, 4);

-- Préstamo activo (descuenta existencia)
INSERT INTO dbo.Prestamos (IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado)
VALUES (1, 1, DATEADD(DAY, -5, CAST(GETDATE() AS DATE)), NULL, N'Prestado');
UPDATE dbo.Libros SET Existencias = Existencias - 1 WHERE IdLibro = 1;

-- Préstamo devuelto (no altera existencias)
INSERT INTO dbo.Prestamos (IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado)
VALUES (2, 3, DATEADD(DAY, -15, CAST(GETDATE() AS DATE)), DATEADD(DAY, -8, CAST(GETDATE() AS DATE)), N'Devuelto');
GO

/* Verificación rápida */
SELECT * FROM dbo.vw_PrestamosDetalle;
GO
