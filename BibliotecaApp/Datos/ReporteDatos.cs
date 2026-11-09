using System.Data;

namespace BibliotecaApp.Datos;

// Consultas que alimentan el módulo de reportes
public class ReporteDatos
{
    public DataTable ListadoLibros()
    {
        string sql = "SELECT l.Codigo AS Código, l.Titulo AS Título, a.Nombre + ' ' + a.Apellido AS Autor, " +
                     "       e.Nombre AS Editorial, l.AnioPublicacion AS Año, l.Existencias " +
                     "FROM Libros l " +
                     "INNER JOIN Autores a ON a.IdAutor = l.IdAutor " +
                     "INNER JOIN Editoriales e ON e.IdEditorial = l.IdEditorial " +
                     "ORDER BY l.Titulo";
        return ConexionBD.Consultar(sql);
    }

    public DataTable ListadoAutores()
    {
        string sql = "SELECT a.Nombre, a.Apellido, COUNT(l.IdLibro) AS [Libros registrados] " +
                     "FROM Autores a LEFT JOIN Libros l ON l.IdAutor = a.IdAutor " +
                     "GROUP BY a.Nombre, a.Apellido ORDER BY a.Apellido";
        return ConexionBD.Consultar(sql);
    }

    public DataTable ListadoEditoriales()
    {
        string sql = "SELECT e.Nombre, e.Pais AS País, COUNT(l.IdLibro) AS [Libros registrados] " +
                     "FROM Editoriales e LEFT JOIN Libros l ON l.IdEditorial = e.IdEditorial " +
                     "GROUP BY e.Nombre, e.Pais ORDER BY e.Nombre";
        return ConexionBD.Consultar(sql);
    }

    public DataTable ListadoUsuarios()
    {
        string sql = "SELECT Documento, Nombre, Apellido, Telefono AS Teléfono, Correo AS [Correo electrónico] " +
                     "FROM Usuarios ORDER BY Apellido, Nombre";
        return ConexionBD.Consultar(sql);
    }

    // estado vacío ("") = todos los préstamos
    public DataTable Prestamos(string estado)
    {
        string sql = "SELECT IdPrestamo AS [N°], Documento, Usuario, Codigo AS Código, Titulo AS Título, " +
                     "       FechaPrestamo AS [Fecha de préstamo], FechaDevolucion AS [Fecha de devolución], Estado " +
                     "FROM vw_PrestamosDetalle " +
                     "WHERE (@estado = '' OR Estado = @estado) " +
                     "ORDER BY IdPrestamo DESC";
        return ConexionBD.Consultar(sql, ConexionBD.Parametro("@estado", estado));
    }

    public DataTable HistorialPorUsuario(string filtro)
    {
        string sql = "SELECT IdPrestamo AS [N°], Documento, Usuario, Titulo AS Título, " +
                     "       FechaPrestamo AS [Fecha de préstamo], FechaDevolucion AS [Fecha de devolución], Estado " +
                     "FROM vw_PrestamosDetalle " +
                     "WHERE Documento LIKE @filtro OR Usuario LIKE @filtro " +
                     "ORDER BY FechaPrestamo DESC";
        return ConexionBD.Consultar(sql, ConexionBD.Parametro("@filtro", "%" + filtro + "%"));
    }

    public DataTable Consolidado()
    {
        string sql = "SELECT N'Libros registrados' AS Indicador, COUNT(*) AS Valor FROM Libros " +
                     "UNION ALL SELECT N'Ejemplares disponibles', ISNULL(SUM(Existencias), 0) FROM Libros " +
                     "UNION ALL SELECT N'Autores', COUNT(*) FROM Autores " +
                     "UNION ALL SELECT N'Editoriales', COUNT(*) FROM Editoriales " +
                     "UNION ALL SELECT N'Usuarios', COUNT(*) FROM Usuarios " +
                     "UNION ALL SELECT N'Préstamos activos', COUNT(*) FROM Prestamos WHERE Estado = 'Prestado' " +
                     "UNION ALL SELECT N'Préstamos devueltos', COUNT(*) FROM Prestamos WHERE Estado = 'Devuelto'";
        return ConexionBD.Consultar(sql);
    }
}
