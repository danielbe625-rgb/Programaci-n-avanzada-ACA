using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Negocio;

// Convierte los errores técnicos de SQL Server en mensajes fáciles de entender.
public static class ErroresBD
{
    public static string Traducir(SqlException ex)
    {
        // 2627 y 2601: se intentó repetir un dato que debe ser único
        if (ex.Number == 2627 || ex.Number == 2601)
        {
            if (ex.Message.Contains("UQ_Usuarios_Documento")) return "Ya existe un usuario con ese documento.";
            if (ex.Message.Contains("UQ_Libros_Codigo")) return "Ya existe un libro con ese código.";
            if (ex.Message.Contains("UQ_Autores")) return "Ya existe un autor con ese nombre y apellido.";
            if (ex.Message.Contains("UQ_Editoriales")) return "Ya existe una editorial con ese nombre.";
            return "Ya existe un registro con esos datos.";
        }

        // 547: choca con una clave foránea o una restricción CHECK
        if (ex.Number == 547)
        {
            return "No se puede completar la operación: el registro está relacionado con otros datos " +
                   "(por ejemplo, tiene libros o préstamos asociados) o el dato indicado no es válido.";
        }

        return "Error de base de datos: " + ex.Message;
    }
}
