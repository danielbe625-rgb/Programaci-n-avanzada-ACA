using System.Data;
using System.Text.RegularExpressions;
using BibliotecaApp.Datos;
using BibliotecaApp.Entidades;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Negocio;

public class UsuarioNegocio
{
    private UsuarioDatos datos = new UsuarioDatos();

    public DataTable Listar(string filtro)
    {
        return datos.Listar(filtro);
    }

    public DataTable ListarParaCombo()
    {
        return datos.ListarParaCombo();
    }

    public void Guardar(Usuario usuario)
    {
        if (usuario.Nombre == "")
            throw new Exception("El nombre es obligatorio.");
        if (usuario.Apellido == "")
            throw new Exception("El apellido es obligatorio.");
        if (string.IsNullOrWhiteSpace(usuario.Documento))
            throw new Exception("El documento es obligatorio.");
        if (!Regex.IsMatch(usuario.Documento, "^[A-Za-z0-9]{5,20}$"))
            throw new Exception("El documento debe tener entre 5 y 20 letras o números, sin espacios.");
        if (usuario.Nombre.Length > 60 || usuario.Apellido.Length > 60)
            throw new Exception("El nombre y el apellido no pueden superar los 60 caracteres.");

        // Teléfono y correo son opcionales
        if (string.IsNullOrWhiteSpace(usuario.Telefono))
            usuario.Telefono = null;
        else if (!Regex.IsMatch(usuario.Telefono, @"^[0-9+\-\s()]{5,20}$"))
            throw new Exception("El teléfono solo puede tener números y los símbolos + - ( ), entre 5 y 20 caracteres.");

        if (string.IsNullOrWhiteSpace(usuario.Correo))
            usuario.Correo = null;
        else if (usuario.Correo.Length > 100 || !Regex.IsMatch(usuario.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new Exception("El correo electrónico no es válido (ejemplo: nombre@dominio.com).");

        try
        {
            if (usuario.IdUsuario == 0)
                datos.Insertar(usuario);
            else
                datos.Actualizar(usuario);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Eliminar(int idUsuario)
    {
        if (idUsuario == 0)
            throw new Exception("Seleccione un usuario de la lista.");

        try
        {
            datos.Eliminar(idUsuario);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }
}
