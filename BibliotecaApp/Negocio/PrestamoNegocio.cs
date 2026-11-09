using System.Data;
using BibliotecaApp.Datos;
using BibliotecaApp.Entidades;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Negocio;

// Reglas de préstamos y devoluciones:
//  - El usuario y el libro deben existir.
//  - No se presta un libro sin existencias.
//  - Las fechas deben ser coherentes.
//  - Estado "Prestado" = sin fecha de devolución; "Devuelto" = con fecha de devolución.
public class PrestamoNegocio
{
    private PrestamoDatos datos = new PrestamoDatos();
    private UsuarioDatos usuarios = new UsuarioDatos();
    private LibroDatos libros = new LibroDatos();

    public DataTable Listar(string filtro, string estado)
    {
        return datos.Listar(filtro, estado);
    }

    public void Registrar(Prestamo prestamo)
    {
        if (prestamo.IdUsuario == 0)
            throw new Exception("Debe seleccionar un usuario.");
        if (prestamo.IdLibro == 0)
            throw new Exception("Debe seleccionar un libro.");
        if (prestamo.FechaPrestamo.Date > DateTime.Today)
            throw new Exception("La fecha de préstamo no puede ser posterior a hoy.");

        if (!usuarios.Existe(prestamo.IdUsuario))
            throw new Exception("El usuario seleccionado no existe.");

        int existencias = libros.ObtenerExistencias(prestamo.IdLibro);
        if (existencias == -1)
            throw new Exception("El libro seleccionado no existe.");
        if (existencias == 0)
            throw new Exception("El libro seleccionado no tiene existencias disponibles.");

        try
        {
            datos.Registrar(prestamo);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Devolver(int idPrestamo, DateTime fechaDevolucion)
    {
        if (idPrestamo == 0)
            throw new Exception("Seleccione un préstamo de la lista.");
        if (fechaDevolucion.Date > DateTime.Today)
            throw new Exception("La fecha de devolución no puede ser posterior a hoy.");

        DataTable tabla = datos.ObtenerPorId(idPrestamo);
        if (tabla.Rows.Count == 0)
            throw new Exception("El préstamo seleccionado no existe.");

        DataRow fila = tabla.Rows[0];
        if (fila["Estado"].ToString() == Prestamo.Devuelto)
            throw new Exception("Este préstamo ya fue devuelto.");

        DateTime fechaPrestamo = Convert.ToDateTime(fila["FechaPrestamo"]);
        if (fechaDevolucion.Date < fechaPrestamo.Date)
            throw new Exception("La fecha de devolución no puede ser anterior a la fecha de préstamo.");

        int idLibro = Convert.ToInt32(fila["IdLibro"]);

        try
        {
            datos.Devolver(idPrestamo, idLibro, fechaDevolucion);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Actualizar(int idPrestamo, int idUsuario, DateTime fechaPrestamo)
    {
        if (idPrestamo == 0)
            throw new Exception("Seleccione un préstamo de la lista.");
        if (idUsuario == 0)
            throw new Exception("Debe seleccionar un usuario.");
        if (fechaPrestamo.Date > DateTime.Today)
            throw new Exception("La fecha de préstamo no puede ser posterior a hoy.");
        if (!usuarios.Existe(idUsuario))
            throw new Exception("El usuario seleccionado no existe.");

        try
        {
            int filas = datos.Actualizar(idPrestamo, idUsuario, fechaPrestamo);
            if (filas == 0)
                throw new Exception("Solo se pueden editar préstamos que estén en estado 'Prestado'.");
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }

    public void Eliminar(int idPrestamo)
    {
        if (idPrestamo == 0)
            throw new Exception("Seleccione un préstamo de la lista.");

        DataTable tabla = datos.ObtenerPorId(idPrestamo);
        if (tabla.Rows.Count == 0)
            throw new Exception("El préstamo seleccionado no existe.");

        int idLibro = Convert.ToInt32(tabla.Rows[0]["IdLibro"]);
        bool estabaPrestado = tabla.Rows[0]["Estado"].ToString() == Prestamo.Prestado;

        try
        {
            datos.Eliminar(idPrestamo, idLibro, estabaPrestado);
        }
        catch (SqlException ex)
        {
            throw new Exception(ErroresBD.Traducir(ex));
        }
    }
}
