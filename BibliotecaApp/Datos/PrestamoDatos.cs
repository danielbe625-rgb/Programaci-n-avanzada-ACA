using System.Data;
using BibliotecaApp.Entidades;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Datos;

public class PrestamoDatos
{
    // estado vacío ("") = todos los estados
    public DataTable Listar(string filtro, string estado)
    {
        string sql = "SELECT p.IdPrestamo, u.Nombre + ' ' + u.Apellido AS Usuario, l.Titulo AS Libro, " +
                     "       p.FechaPrestamo, p.FechaDevolucion, p.Estado, p.IdUsuario, p.IdLibro " +
                     "FROM Prestamos p " +
                     "INNER JOIN Usuarios u ON u.IdUsuario = p.IdUsuario " +
                     "INNER JOIN Libros l ON l.IdLibro = p.IdLibro " +
                     "WHERE (@estado = '' OR p.Estado = @estado) " +
                     "  AND (u.Documento LIKE @filtro OR u.Nombre LIKE @filtro OR u.Apellido LIKE @filtro " +
                     "       OR l.Titulo LIKE @filtro OR l.Codigo LIKE @filtro) " +
                     "ORDER BY p.IdPrestamo DESC";
        return ConexionBD.Consultar(sql,
            ConexionBD.Parametro("@filtro", "%" + filtro + "%"),
            ConexionBD.Parametro("@estado", estado));
    }

    public DataTable ObtenerPorId(int idPrestamo)
    {
        return ConexionBD.Consultar(
            "SELECT IdPrestamo, IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado FROM Prestamos WHERE IdPrestamo = @id",
            ConexionBD.Parametro("@id", idPrestamo));
    }

    // Registra el préstamo y descuenta una existencia del libro.
    // Se usa una transacción: o se hacen las dos operaciones o no se hace ninguna.
    public void Registrar(Prestamo prestamo)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        {
            conexion.Open();
            SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                SqlCommand descontar = new SqlCommand(
                    "UPDATE Libros SET Existencias = Existencias - 1 WHERE IdLibro = @idLibro AND Existencias > 0",
                    conexion, transaccion);
                descontar.Parameters.Add(ConexionBD.Parametro("@idLibro", prestamo.IdLibro));
                if (descontar.ExecuteNonQuery() == 0)
                {
                    throw new Exception("El libro seleccionado no tiene existencias disponibles.");
                }

                SqlCommand insertar = new SqlCommand(
                    "INSERT INTO Prestamos (IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado) " +
                    "VALUES (@idUsuario, @idLibro, @fecha, NULL, @estado)",
                    conexion, transaccion);
                insertar.Parameters.Add(ConexionBD.Parametro("@idUsuario", prestamo.IdUsuario));
                insertar.Parameters.Add(ConexionBD.Parametro("@idLibro", prestamo.IdLibro));
                insertar.Parameters.Add(ConexionBD.Parametro("@fecha", prestamo.FechaPrestamo.Date));
                insertar.Parameters.Add(ConexionBD.Parametro("@estado", Prestamo.Prestado));
                insertar.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }
    }

    // Registra la devolución: cambia el estado a "Devuelto", guarda la fecha y suma una existencia.
    public void Devolver(int idPrestamo, int idLibro, DateTime fechaDevolucion)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        {
            conexion.Open();
            SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                SqlCommand actualizarPrestamo = new SqlCommand(
                    "UPDATE Prestamos SET FechaDevolucion = @fecha, Estado = @estadoNuevo " +
                    "WHERE IdPrestamo = @id AND Estado = @estadoActual",
                    conexion, transaccion);
                actualizarPrestamo.Parameters.Add(ConexionBD.Parametro("@fecha", fechaDevolucion.Date));
                actualizarPrestamo.Parameters.Add(ConexionBD.Parametro("@estadoNuevo", Prestamo.Devuelto));
                actualizarPrestamo.Parameters.Add(ConexionBD.Parametro("@id", idPrestamo));
                actualizarPrestamo.Parameters.Add(ConexionBD.Parametro("@estadoActual", Prestamo.Prestado));
                if (actualizarPrestamo.ExecuteNonQuery() == 0)
                {
                    throw new Exception("El préstamo no existe o ya fue devuelto.");
                }

                SqlCommand sumar = new SqlCommand(
                    "UPDATE Libros SET Existencias = Existencias + 1 WHERE IdLibro = @idLibro",
                    conexion, transaccion);
                sumar.Parameters.Add(ConexionBD.Parametro("@idLibro", idLibro));
                sumar.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }
    }

    // Solo permite editar préstamos que siguen en estado "Prestado"
    public int Actualizar(int idPrestamo, int idUsuario, DateTime fechaPrestamo)
    {
        string sql = "UPDATE Prestamos SET IdUsuario = @idUsuario, FechaPrestamo = @fecha " +
                     "WHERE IdPrestamo = @id AND Estado = @estado";
        return ConexionBD.Ejecutar(sql,
            ConexionBD.Parametro("@idUsuario", idUsuario),
            ConexionBD.Parametro("@fecha", fechaPrestamo.Date),
            ConexionBD.Parametro("@id", idPrestamo),
            ConexionBD.Parametro("@estado", Prestamo.Prestado));
    }

    // Elimina el préstamo. Si el libro todavía estaba prestado, se le devuelve la existencia.
    public void Eliminar(int idPrestamo, int idLibro, bool estabaPrestado)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        {
            conexion.Open();
            SqlTransaction transaccion = conexion.BeginTransaction();
            try
            {
                SqlCommand eliminar = new SqlCommand("DELETE FROM Prestamos WHERE IdPrestamo = @id",
                    conexion, transaccion);
                eliminar.Parameters.Add(ConexionBD.Parametro("@id", idPrestamo));
                eliminar.ExecuteNonQuery();

                if (estabaPrestado)
                {
                    SqlCommand sumar = new SqlCommand(
                        "UPDATE Libros SET Existencias = Existencias + 1 WHERE IdLibro = @idLibro",
                        conexion, transaccion);
                    sumar.Parameters.Add(ConexionBD.Parametro("@idLibro", idLibro));
                    sumar.ExecuteNonQuery();
                }

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                throw;
            }
        }
    }
}
