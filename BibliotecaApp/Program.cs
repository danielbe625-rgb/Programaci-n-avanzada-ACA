using BibliotecaApp.Datos;
using BibliotecaApp.Presentacion;

namespace BibliotecaApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Antes de abrir la aplicación se verifica la conexión con SQL Server
        string mensaje;
        if (!ConexionBD.ProbarConexion(out mensaje))
        {
            MessageBox.Show(
                "No se pudo conectar con la base de datos BibliotecaDB.\n\n" +
                "Revise que SQL Server esté encendido, que haya ejecutado el script " +
                "BibliotecaDB.sql y que la cadena de conexión (archivo Datos/ConexionBD.cs) sea correcta.\n\n" +
                "Detalle: " + mensaje,
                "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Application.Run(new FrmPrincipal());
    }
}
