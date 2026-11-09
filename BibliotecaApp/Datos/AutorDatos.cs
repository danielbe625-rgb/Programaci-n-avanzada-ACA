using System.Data;
using BibliotecaApp.Entidades;

namespace BibliotecaApp.Datos;

public class AutorDatos
{
    public DataTable Listar(string filtro)
    {
        string sql = "SELECT IdAutor, Nombre, Apellido FROM Autores " +
                     "WHERE Nombre LIKE @filtro OR Apellido LIKE @filtro " +
                     "ORDER BY Apellido, Nombre";
        return ConexionBD.Consultar(sql, ConexionBD.Parametro("@filtro", "%" + filtro + "%"));
    }

    // Lista simple para llenar ComboBox
    public DataTable ListarParaCombo()
    {
        string sql = "SELECT IdAutor, Nombre + ' ' + Apellido AS NombreCompleto FROM Autores ORDER BY Apellido, Nombre";
        return ConexionBD.Consultar(sql);
    }

    public int Insertar(Autor autor)
    {
        string sql = "INSERT INTO Autores (Nombre, Apellido) VALUES (@nombre, @apellido)";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@nombre", autor.Nombre),
            ConexionBD.Parametro("@apellido", autor.Apellido));
    }

    public int Actualizar(Autor autor)
    {
        string sql = "UPDATE Autores SET Nombre = @nombre, Apellido = @apellido WHERE IdAutor = @id";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@nombre", autor.Nombre),
            ConexionBD.Parametro("@apellido", autor.Apellido),
            ConexionBD.Parametro("@id", autor.IdAutor));
    }

    public int Eliminar(int idAutor)
    {
        return ConexionBD.Ejecutar("DELETE FROM Autores WHERE IdAutor = @id",
            ConexionBD.Parametro("@id", idAutor));
    }

    public bool Existe(int idAutor)
    {
        object cantidad = ConexionBD.EjecutarEscalar("SELECT COUNT(*) FROM Autores WHERE IdAutor = @id",
            ConexionBD.Parametro("@id", idAutor));
        return Convert.ToInt32(cantidad) > 0;
    }
}
