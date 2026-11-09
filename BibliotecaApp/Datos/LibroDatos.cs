using System.Data;
using BibliotecaApp.Entidades;

namespace BibliotecaApp.Datos;

public class LibroDatos
{
    public DataTable Listar(string filtro)
    {
        string sql = "SELECT l.IdLibro, l.Codigo, l.Titulo, " +
                     "       a.Nombre + ' ' + a.Apellido AS Autor, " +
                     "       e.Nombre AS Editorial, " +
                     "       l.AnioPublicacion AS Anio, l.Existencias, " +
                     "       l.IdAutor, l.IdEditorial " +
                     "FROM Libros l " +
                     "INNER JOIN Autores a ON a.IdAutor = l.IdAutor " +
                     "INNER JOIN Editoriales e ON e.IdEditorial = l.IdEditorial " +
                     "WHERE l.Codigo LIKE @filtro OR l.Titulo LIKE @filtro " +
                     "   OR (a.Nombre + ' ' + a.Apellido) LIKE @filtro " +
                     "ORDER BY l.Titulo";
        return ConexionBD.Consultar(sql, ConexionBD.Parametro("@filtro", "%" + filtro + "%"));
    }

    // Lista simple para llenar ComboBox
    public DataTable ListarParaCombo()
    {
        string sql = "SELECT IdLibro, Codigo + ' - ' + Titulo + ' (Existencias: ' + CAST(Existencias AS NVARCHAR(10)) + ')' AS Descripcion " +
                     "FROM Libros ORDER BY Titulo";
        return ConexionBD.Consultar(sql);
    }

    public int Insertar(Libro libro)
    {
        string sql = "INSERT INTO Libros (Codigo, Titulo, IdAutor, IdEditorial, AnioPublicacion, Existencias) " +
                     "VALUES (@codigo, @titulo, @idAutor, @idEditorial, @anio, @existencias)";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@codigo", libro.Codigo),
            ConexionBD.Parametro("@titulo", libro.Titulo),
            ConexionBD.Parametro("@idAutor", libro.IdAutor),
            ConexionBD.Parametro("@idEditorial", libro.IdEditorial),
            ConexionBD.Parametro("@anio", libro.AnioPublicacion),
            ConexionBD.Parametro("@existencias", libro.Existencias));
    }

    public int Actualizar(Libro libro)
    {
        string sql = "UPDATE Libros SET Codigo = @codigo, Titulo = @titulo, IdAutor = @idAutor, " +
                     "IdEditorial = @idEditorial, AnioPublicacion = @anio, Existencias = @existencias " +
                     "WHERE IdLibro = @id";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@codigo", libro.Codigo),
            ConexionBD.Parametro("@titulo", libro.Titulo),
            ConexionBD.Parametro("@idAutor", libro.IdAutor),
            ConexionBD.Parametro("@idEditorial", libro.IdEditorial),
            ConexionBD.Parametro("@anio", libro.AnioPublicacion),
            ConexionBD.Parametro("@existencias", libro.Existencias),
            ConexionBD.Parametro("@id", libro.IdLibro));
    }

    public int Eliminar(int idLibro)
    {
        return ConexionBD.Ejecutar("DELETE FROM Libros WHERE IdLibro = @id",
            ConexionBD.Parametro("@id", idLibro));
    }

    // Devuelve las existencias del libro, o -1 si el libro no existe
    public int ObtenerExistencias(int idLibro)
    {
        object resultado = ConexionBD.EjecutarEscalar("SELECT Existencias FROM Libros WHERE IdLibro = @id",
            ConexionBD.Parametro("@id", idLibro));
        if (resultado == null)
        {
            return -1;
        }
        return Convert.ToInt32(resultado);
    }
}
