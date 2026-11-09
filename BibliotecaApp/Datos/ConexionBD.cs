using System.Data;
using Microsoft.Data.SqlClient;

namespace BibliotecaApp.Datos;

// Maneja la conexión con SQL Server y ejecuta las consultas parametrizadas.
public static class ConexionBD
{
    // CADENA DE CONEXIÓN: cámbiela si su servidor es distinto.
    //  - Autenticación de Windows (por defecto):
    //      Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;
    //  - Usuario y contraseña de SQL Server:
    //      Server=localhost;Database=BibliotecaDB;User Id=sa;Password=SU_CLAVE;TrustServerCertificate=True;
    //  - SQL Server Express: use  Server=localhost\SQLEXPRESS;
    private static string cadenaConexion =
        "Server=localhost;Database=BibliotecaDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(cadenaConexion);
    }

    public static bool ProbarConexion(out string mensaje)
    {
        try
        {
            using (SqlConnection conexion = ObtenerConexion())
            {
                conexion.Open();
            }
            mensaje = "Conexión exitosa.";
            return true;
        }
        catch (Exception ex)
        {
            mensaje = ex.Message;
            return false;
        }
    }

    // Crea un parámetro SQL. Si el valor es null se guarda como NULL en la base de datos.
    public static SqlParameter Parametro(string nombre, object valor)
    {
        if (valor == null)
        {
            valor = DBNull.Value;
        }
        return new SqlParameter(nombre, valor);
    }

    // Ejecuta un SELECT y devuelve una tabla con los resultados.
    public static DataTable Consultar(string sql, params SqlParameter[] parametros)
    {
        DataTable tabla = new DataTable();
        using (SqlConnection conexion = ObtenerConexion())
        {
            SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion);
            adaptador.SelectCommand.Parameters.AddRange(parametros);
            adaptador.Fill(tabla);
        }
        return tabla;
    }

    // Ejecuta INSERT, UPDATE o DELETE y devuelve cuántas filas se afectaron.
    public static int Ejecutar(string sql, params SqlParameter[] parametros)
    {
        using (SqlConnection conexion = ObtenerConexion())
        {
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddRange(parametros);
            conexion.Open();
            return comando.ExecuteNonQuery();
        }
    }

    // Ejecuta una consulta que devuelve un solo valor (por ejemplo un COUNT).
    public static object EjecutarEscalar(string sql, params SqlParameter[] parametros)
    {
        using (SqlConnection conexion = ObtenerConexion())
        {
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.AddRange(parametros);
            conexion.Open();
            return comando.ExecuteScalar();
        }
    }
}
