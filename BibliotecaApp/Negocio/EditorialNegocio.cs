using System.Data;
using BibliotecaApp.Datos;
using BibliotecaApp.Entidades;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Negocio;

public class EditorialNegocio
{
    private EditorialDatos datos = new EditorialDatos();

    public DataTable Listar(string filtro)
    {
        return datos.Listar(filtro);
    }

    public void Guardar(Editorial editorial)
    {
        if (string.IsNullOrWhiteSpace(editorial.Nombre))
            throw new Exception("El nombre de la editorial es obligatorio.");
        if (editorial.Nombre.Length > 100)
            throw new Exception("El nombre no puede superar los 100 caracteres.");
        if (editorial.Pais != null && editorial.Pais.Length > 60)
            throw new Exception("El país no puede superar los 60 caracteres.");

        // El país es opcional: si está vacío se guarda como NULL
        if (string.IsNullOrWhiteSpace(editorial.Pais))
            editorial.Pais = null;

        try
        {
            if (editorial.IdEditorial == 0)
                datos.Insertar(editorial);
            else
                datos.Actualizar(editorial);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Eliminar(int idEditorial)
    {
        if (idEditorial == 0)
            throw new Exception("Seleccione una editorial de la lista.");

        try
        {
            datos.Eliminar(idEditorial);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }
}
