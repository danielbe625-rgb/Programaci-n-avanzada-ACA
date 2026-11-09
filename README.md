# Sistema de Gestión de Biblioteca

Aplicación de escritorio desarrollada en **C# (.NET 8) con Windows Forms** y **SQL Server** para la
gestión integral de una biblioteca: libros, autores, editoriales, usuarios, préstamos, devoluciones
y reportes. Proyecto final de **Programación Avanzada**.

## Funcionalidades

- Formulario principal con navegación entre módulos (Inicio, Libros, Usuarios, Autores, Editoriales, Préstamos, Reportes).
- CRUD completo de Libros, Autores, Editoriales y Usuarios, con búsqueda.
- Registro, edición y eliminación de préstamos; gestión de devoluciones con control de existencias.
- Estado del préstamo (`Prestado` / `Devuelto`) coherente con la fecha de devolución.
- Módulo de reportes (9 reportes) con exportación a CSV.
- Validaciones de datos, prevención de duplicados y manejo de excepciones con mensajes claros.
- Consultas parametrizadas y transacciones en préstamos/devoluciones.

## Arquitectura (por capas)

```
BibliotecaApp/
├── Presentacion/   Formularios Windows Forms (FrmPrincipal, FrmLibros, FrmPrestamos, ...)
├── Negocio/        Reglas del sistema y validaciones (LibroNegocio, PrestamoNegocio, ...)
├── Datos/          Acceso a SQL Server con consultas parametrizadas (LibroDatos, ConexionBD, ...)
└── Entidades/      Clases del dominio (Persona, Autor, Usuario, Libro, Prestamo, ...)
database/           Script SQL (BibliotecaDB.sql)
docs/               Documento técnico y capturas de pantalla
```

Flujo: `Presentacion → Negocio → Datos → SQL Server`.

## Requisitos

- Windows 10/11 (Windows Forms solo se ejecuta en Windows).
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (Express o Developer) y un cliente como DBeaver o SSMS.
- Visual Studio Code con la extensión **C# Dev Kit** (o Visual Studio 2022).

## Instrucciones de ejecución

1. **Crear la base de datos:** abra `database/BibliotecaDB.sql` en DBeaver/SSMS y ejecútelo completo
   (en DBeaver use *Alt + X*). Crea `BibliotecaDB`, las tablas, restricciones, vista y datos de prueba.
2. **Configurar la conexión:** abra `BibliotecaApp/Datos/ConexionBD.cs` y revise la variable `cadenaConexion`.
   - Autenticación de Windows (por defecto):
     `Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;`
   - Usuario y contraseña de SQL Server:
     `Server=localhost;Database=BibliotecaDB;User Id=sa;Password=SU_CLAVE;TrustServerCertificate=True;`
   - SQL Server Express con instancia nombrada: use `Server=localhost\\SQLEXPRESS;...` (doble barra dentro del texto de C#).
3. **Ejecutar** desde la carpeta raíz del repositorio:

   ```bash
   dotnet restore BibliotecaApp/BibliotecaApp.csproj
   dotnet run --project BibliotecaApp/BibliotecaApp.csproj
   ```

   También puede abrir la carpeta en VS Code y pulsar **F5** (usa `.vscode/launch.json`).

## Base de datos

Tablas: `Autores`, `Editoriales`, `Usuarios`, `Libros`, `Prestamos` y la vista `vw_PrestamosDetalle`.
Las claves foráneas no tienen eliminación en cascada: no se puede borrar un registro con relaciones.

## Control de versiones

Se recomienda realizar commits frecuentes y descriptivos, por ejemplo:

```
feat: script SQL con tablas, restricciones y datos de prueba
feat: entidades del dominio con herencia y encapsulamiento
feat: capa de acceso a datos con consultas parametrizadas
feat: capa de negocio y validaciones
feat: formularios CRUD de autores, editoriales y usuarios
feat: formulario de libros con relación a autores y editoriales
feat: gestión de préstamos y devoluciones con transacciones
feat: módulo de reportes y exportación a CSV
docs: README, documento técnico y capturas de pantalla
```

## Autor

Nombre del estudiante — Programación Avanzada, 2026.
