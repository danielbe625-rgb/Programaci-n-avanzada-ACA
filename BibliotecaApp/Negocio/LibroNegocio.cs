using System.Data;
using BibliotecaApp.Datos;
using BibliotecaApp.Entidades;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Negocio;

public class LibroNegocio
{
    private LibroDatos datos = new LibroDatos();
    private AutorDatos autores = new AutorDatos();
    private EditorialDatos editoriales = new EditorialDatos();

    public DataTable Listar(string filtro)
    {
        return datos.Listar(filtro);
    }

    public DataTable ListarParaCombo()
    {
        return datos.ListarParaCombo();
    }

    public void Guardar(Libro libro)
    {
        if (string.IsNullOrWhiteSpace(libro.Codigo))
            throw new Exception("El código del libro es obligatorio.");
        if (string.IsNullOrWhiteSpace(libro.Titulo))
            throw new Exception("El título del libro es obligatorio.");
        if (libro.Codigo.Length > 20)
            throw new Exception("El código no puede superar los 20 caracteres.");
        if (libro.Titulo.Length > 150)
            throw new Exception("El título no puede superar los 150 caracteres.");
        if (libro.IdAutor == 0)
            throw new Exception("Debe seleccionar un autor.");
        if (libro.IdEditorial == 0)
            throw new Exception("Debe seleccionar una editorial.");
        if (libro.AnioPublicacion != null && (libro.AnioPublicacion < 1000 || libro.AnioPublicacion > DateTime.Today.Year + 1))
            throw new Exception("El año de publicación no es válido.");
        if (libro.Existencias < 0)
            throw new Exception("Las existencias no pueden ser negativas.");

        // El autor y la editorial deben existir en la base de datos
        if (!autores.Existe(libro.IdAutor))
            throw new Exception("El autor seleccionado no existe.");
        if (!editoriales.Existe(libro.IdEditorial))
            throw new Exception("La editorial seleccionada no existe.");

        try
        {
            if (libro.IdLibro == 0)
                datos.Insertar(libro);
            else
                datos.Actualizar(libro);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Eliminar(int idLibro)
    {
        if (idLibro == 0)
            throw new Exception("Seleccione un libro de la lista.");

        try
        {
            datos.Eliminar(idLibro);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }
}
