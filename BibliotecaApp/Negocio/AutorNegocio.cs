using System.Data;
using BibliotecaApp.Datos;
using BibliotecaApp.Entidades;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Negocio;

public class AutorNegocio
{
    private AutorDatos datos = new AutorDatos();

    public DataTable Listar(string filtro)
    {
        return datos.Listar(filtro);
    }

    public DataTable ListarParaCombo()
    {
        return datos.ListarParaCombo();
    }

    // Si IdAutor es 0 se registra un autor nuevo; si no, se actualiza el existente.
    public void Guardar(Autor autor)
    {
        if (autor.Nombre == "")
            throw new Exception("El nombre es obligatorio.");
        if (autor.Apellido == "")
            throw new Exception("El apellido es obligatorio.");
        if (autor.Nombre.Length > 60 || autor.Apellido.Length > 60)
            throw new Exception("El nombre y el apellido no pueden superar los 60 caracteres.");

        try
        {
            if (autor.IdAutor == 0)
                datos.Insertar(autor);
            else
                datos.Actualizar(autor);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Eliminar(int idAutor)
    {
        if (idAutor == 0)
            throw new Exception("Seleccione un autor de la lista.");

        try
        {
            datos.Eliminar(idAutor);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }
}
