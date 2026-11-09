using System.Data;
using BibliotecaApp.Entidades;

namespace BibliotecaApp.Datos;

public class EditorialDatos
{
    public DataTable Listar(string filtro)
    {
        string sql = "SELECT IdEditorial, Nombre, Pais FROM Editoriales " +
                     "WHERE Nombre LIKE @filtro OR Pais LIKE @filtro " +
                     "ORDER BY Nombre";
        return ConexionBD.Consultar(sql, ConexionBD.Parametro("@filtro", "%" + filtro + "%"));
    }

    public int Insertar(Editorial editorial)
    {
        string sql = "INSERT INTO Editoriales (Nombre, Pais) VALUES (@nombre, @pais)";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@nombre", editorial.Nombre),
            ConexionBD.Parametro("@pais", editorial.Pais));
    }

    public int Actualizar(Editorial editorial)
    {
        string sql = "UPDATE Editoriales SET Nombre = @nombre, Pais = @pais WHERE IdEditorial = @id";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@nombre", editorial.Nombre),
            ConexionBD.Parametro("@pais", editorial.Pais),
            ConexionBD.Parametro("@id", editorial.IdEditorial));
    }

    public int Eliminar(int idEditorial)
    {
        return ConexionBD.Ejecutar("DELETE FROM Editoriales WHERE IdEditorial = @id",
            ConexionBD.Parametro("@id", idEditorial));
    }

    public bool Existe(int idEditorial)
    {
        object cantidad = ConexionBD.EjecutarEscalar("SELECT COUNT(*) FROM Editoriales WHERE IdEditorial = @id",
            ConexionBD.Parametro("@id", idEditorial));
        return Convert.ToInt32(cantidad) > 0;
    }
}
