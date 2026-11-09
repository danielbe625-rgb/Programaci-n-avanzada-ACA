using System.Data;
using BibliotecaApp.Negocio;

namespace BibliotecaApp.Presentacion;

// Módulo de consultas y reportes
public class FrmReportes : Form
{
    private ReporteNegocio negocio = new ReporteNegocio();
    private DataTable tablaActual = null;

    private Panel pnlSuperior = new Panel();
    private DataGridView dgvReporte = new DataGridView();
    private Label lblTotal = new Label();
    private ComboBox cmbReporte;
    private TextBox txtFiltro;
    private Button btnGenerar;
    private Button btnExportar;

    public FrmReportes()
    {
        Text = "Reportes";
        BackColor = Interfaz.ColorFondo;

        pnlSuperior.Dock = DockStyle.Top;
        pnlSuperior.Height = 100;

        cmbReporte = Interfaz.CrearCombo(110, 20, 360);
        foreach (string nombre in ReporteNegocio.NombresReportes)
        {
            cmbReporte.Items.Add(nombre);
        }
        cmbReporte.SelectedIndex = 0;

        txtFiltro = Interfaz.CrearCaja(110, 60, 360, 100);
        btnGenerar = Interfaz.CrearBoton("Generar", 490, 16, 110, Interfaz.ColorVerde);
        btnExportar = Interfaz.CrearBoton("Exportar CSV", 610, 16, 130, Interfaz.ColorAzul);

        pnlSuperior.Controls.Add(Interfaz.CrearEtiqueta("Reporte:", 20, 24));
        pnlSuperior.Controls.Add(cmbReporte);
        pnlSuperior.Controls.Add(Interfaz.CrearEtiqueta("Filtro:", 20, 64));
        pnlSuperior.Controls.Add(txtFiltro);
        pnlSuperior.Controls.Add(Interfaz.CrearEtiqueta("(el filtro se usa en el historial por usuario: documento o nombre)", 490, 64));
        pnlSuperior.Controls.Add(btnGenerar);
        pnlSuperior.Controls.Add(btnExportar);

        Interfaz.ConfigurarGrilla(dgvReporte);

        lblTotal.Dock = DockStyle.Bottom;
        lblTotal.Height = 30;
        lblTotal.TextAlign = ContentAlignment.MiddleLeft;
        lblTotal.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        lblTotal.ForeColor = Interfaz.ColorPrincipal;
        lblTotal.Text = "  Seleccione un reporte y pulse «Generar».";

        Controls.Add(dgvReporte);
        Controls.Add(lblTotal);
        Controls.Add(pnlSuperior);

        btnGenerar.Click += btnGenerar_Click;
        btnExportar.Click += btnExportar_Click;
    }

    private void btnGenerar_Click(object sender, EventArgs e)
    {
        try
        {
            string nombre = cmbReporte.Text;
            tablaActual = negocio.Generar(nombre, txtFiltro.Text.Trim());
            dgvReporte.DataSource = tablaActual;

            // Mostrar las fechas sin la hora
            foreach (DataGridViewColumn columna in dgvReporte.Columns)
            {
                if (columna.ValueType == typeof(DateTime))
                {
                    columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }

            dgvReporte.ClearSelection();
            lblTotal.Text = "  " + nombre + " — Registros encontrados: " + tablaActual.Rows.Count;
        }
        catch (Exception ex)
        {
            Interfaz.Advertencia(ex.Message);
        }
    }

    private void btnExportar_Click(object sender, EventArgs e)
    {
        if (tablaActual == null || tablaActual.Rows.Count == 0)
        {
            Interfaz.Advertencia("Primero genere un reporte que tenga datos.");
            return;
        }

        SaveFileDialog dialogo = new SaveFileDialog();
        dialogo.Filter = "Archivo CSV (*.csv)|*.csv";
        dialogo.FileName = "reporte_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";

        if (dialogo.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            negocio.ExportarCsv(tablaActual, dialogo.FileName);
            Interfaz.Informacion("Reporte exportado correctamente.");
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudo exportar el reporte: " + ex.Message);
        }
    }
}
