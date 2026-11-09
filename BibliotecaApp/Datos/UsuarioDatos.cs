using System.Data;
using BibliotecaApp.Entidades;

namespace BibliotecaApp.Datos;

public class UsuarioDatos
{
    public DataTable Listar(string filtro)
    {
        string sql = "SELECT IdUsuario, Documento, Nombre, Apellido, Telefono, Correo FROM Usuarios " +
                     "WHERE Documento LIKE @filtro OR Nombre LIKE @filtro OR Apellido LIKE @filtro " +
                     "ORDER BY Apellido, Nombre";
        return ConexionBD.Consultar(sql, ConexionBD.Parametro("@filtro", "%" + filtro + "%"));
    }

    // Lista simple para llenar ComboBox
    public DataTable ListarParaCombo()
    {
        string sql = "SELECT IdUsuario, Nombre + ' ' + Apellido + ' - ' + Documento AS NombreCompleto " +
                     "FROM Usuarios ORDER BY Apellido, Nombre";
        return ConexionBD.Consultar(sql);
    }

    public int Insertar(Usuario usuario)
    {
        string sql = "INSERT INTO Usuarios (Nombre, Apellido, Documento, Telefono, Correo) " +
                     "VALUES (@nombre, @apellido, @documento, @telefono, @correo)";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@nombre", usuario.Nombre),
            ConexionBD.Parametro("@apellido", usuario.Apellido),
            ConexionBD.Parametro("@documento", usuario.Documento),
            ConexionBD.Parametro("@telefono", usuario.Telefono),
            ConexionBD.Parametro("@correo", usuario.Correo));
    }

    public int Actualizar(Usuario usuario)
    {
        string sql = "UPDATE Usuarios SET Nombre = @nombre, Apellido = @apellido, Documento = @documento, " +
                     "Telefono = @telefono, Correo = @correo WHERE IdUsuario = @id";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@nombre", usuario.Nombre),
            ConexionBD.Parametro("@apellido", usuario.Apellido),
            ConexionBD.Parametro("@documento", usuario.Documento),
            ConexionBD.Parametro("@telefono", usuario.Telefono),
            ConexionBD.Parametro("@correo", usuario.Correo),
            ConexionBD.Parametro("@id", usuario.IdUsuario));
    }

    public int Eliminar(int idUsuario)
    {
        return ConexionBD.Ejecutar("DELETE FROM Usuarios WHERE IdUsuario = @id",
            ConexionBD.Parametro("@id", idUsuario));
    }

    public bool Existe(int idUsuario)
    {
        object cantidad = ConexionBD.EjecutarEscalar("SELECT COUNT(*) FROM Usuarios WHERE IdUsuario = @id",
            ConexionBD.Parametro("@id", idUsuario));
        return Convert.ToInt32(cantidad) > 0;
    }
}
