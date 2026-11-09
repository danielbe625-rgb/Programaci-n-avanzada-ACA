using System.Data;
using System.Text;
using BibliotecaApp.Datos;

namespace BibliotecaApp.Negocio;

public class ReporteNegocio
{
    private ReporteDatos datos = new ReporteDatos();

    // Nombres de los reportes disponibles (se muestran en el ComboBox)
    public static string[] NombresReportes = new string[]
    {
        "Listado de libros",
        "Listado de autores",
        "Listado de editoriales",
        "Listado de usuarios",
        "Relación de préstamos registrados",
        "Préstamos activos",
        "Préstamos devueltos",
        "Historial de préstamos por usuario",
        "Información consolidada de la biblioteca"
    };

    public DataTable Generar(string reporte, string filtro)
    {
        switch (reporte)
        {
            case "Listado de libros":
                return datos.ListadoLibros();
            case "Listado de autores":
                return datos.ListadoAutores();
            case "Listado de editoriales":
                return datos.ListadoEditoriales();
            case "Listado de usuarios":
                return datos.ListadoUsuarios();
            case "Relación de préstamos registrados":
                return datos.Prestamos("");
            case "Préstamos activos":
                return datos.Prestamos("Prestado");
            case "Préstamos devueltos":
                return datos.Prestamos("Devuelto");
            case "Historial de préstamos por usuario":
                if (string.IsNullOrWhiteSpace(filtro))
                    throw new Exception("Escriba el documento o el nombre del usuario en el campo Filtro.");
                return datos.HistorialPorUsuario(filtro.Trim());
            case "Información consolidada de la biblioteca":
                return datos.Consolidado();
            default:
                throw new Exception("Seleccione un reporte.");
        }
    }

    // Guarda la tabla en un archivo CSV (separado por punto y coma, para abrir en Excel)
    public void ExportarCsv(DataTable tabla, string ruta)
    {
        StringBuilder texto = new StringBuilder();

        // Encabezados
        for (int i = 0; i < tabla.Columns.Count; i++)
        {
            texto.Append(tabla.Columns[i].ColumnName);
            if (i < tabla.Columns.Count - 1) texto.Append(";");
        }
        texto.AppendLine();

        // Filas
        foreach (DataRow fila in tabla.Rows)
        {
            for (int i = 0; i < tabla.Columns.Count; i++)
            {
                object valor = fila[i];
                if (valor is DateTime)
                    texto.Append(((DateTime)valor).ToString("dd/MM/yyyy"));
                else if (valor != DBNull.Value)
                    texto.Append(valor.ToString().Replace(";", ","));
                if (i < tabla.Columns.Count - 1) texto.Append(";");
            }
            texto.AppendLine();
        }

        File.WriteAllText(ruta, texto.ToString(), new UTF8Encoding(true));
    }
}
