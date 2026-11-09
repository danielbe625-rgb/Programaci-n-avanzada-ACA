using System.Data;
using BibliotecaApp.Entidades;
using BibliotecaApp.Negocio;

namespace BibliotecaApp.Presentacion;

public class FrmEditoriales : Form
{
    private EditorialNegocio negocio = new EditorialNegocio();
    private int idSeleccionado = 0;   // 0 = editorial nueva

    private Panel pnlDatos = new Panel();
    private DataGridView dgvEditoriales = new DataGridView();
    private TextBox txtNombre;
    private TextBox txtPais;
    private TextBox txtBuscar;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
    private Button btnBuscar;

    public FrmEditoriales()
    {
        Text = "Editoriales";
        BackColor = Interfaz.ColorFondo;

        pnlDatos.Dock = DockStyle.Top;
        pnlDatos.Height = 150;

        txtNombre = Interfaz.CrearCaja(130, 20, 220, 100);
        txtPais = Interfaz.CrearCaja(520, 20, 220, 60);
        btnNuevo = Interfaz.CrearBoton("Nuevo", 20, 65, 110, Interfaz.ColorAzul);
        btnGuardar = Interfaz.CrearBoton("Guardar", 140, 65, 110, Interfaz.ColorVerde);
        btnEliminar = Interfaz.CrearBoton("Eliminar", 260, 65, 110, Interfaz.ColorRojo);
        txtBuscar = Interfaz.CrearCaja(130, 110, 220, 100);
        btnBuscar = Interfaz.CrearBoton("Buscar", 360, 106, 90, Interfaz.ColorPrincipal);

        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Nombre:", 20, 24));
        pnlDatos.Controls.Add(txtNombre);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("País:", 410, 24));
        pnlDatos.Controls.Add(txtPais);
        pnlDatos.Controls.Add(btnNuevo);
        pnlDatos.Controls.Add(btnGuardar);
        pnlDatos.Controls.Add(btnEliminar);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Buscar:", 20, 114));
        pnlDatos.Controls.Add(txtBuscar);
        pnlDatos.Controls.Add(btnBuscar);

        Interfaz.ConfigurarGrilla(dgvEditoriales);

        Controls.Add(dgvEditoriales);
        Controls.Add(pnlDatos);

        btnNuevo.Click += btnNuevo_Click;
        btnGuardar.Click += btnGuardar_Click;
        btnEliminar.Click += btnEliminar_Click;
        btnBuscar.Click += btnBuscar_Click;
        dgvEditoriales.CellClick += dgvEditoriales_CellClick;
        Load += FrmEditoriales_Load;
    }

    private void FrmEditoriales_Load(object sender, EventArgs e)
    {
        CargarLista("");
    }

    private void CargarLista(string filtro)
    {
        try
        {
            dgvEditoriales.DataSource = negocio.Listar(filtro);
            dgvEditoriales.Columns["IdEditorial"].Visible = false;
            dgvEditoriales.Columns["Pais"].HeaderText = "País";
            dgvEditoriales.ClearSelection();
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudo cargar la lista de editoriales: " + ex.Message);
        }
    }

    private void LimpiarCampos()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        txtPais.Clear();
        txtNombre.Focus();
        dgvEditoriales.ClearSelection();
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = (idSeleccionado == 0);
            Editorial editorial = new Editorial(idSeleccionado, txtNombre.Text.Trim(), txtPais.Text.Trim());
            negocio.Guardar(editorial);

            if (esNuevo)
                Interfaz.Informacion("Editorial registrada correctamente.");
            else
                Interfaz.Informacion("Editorial actualizada correctamente.");

            LimpiarCampos();
            txtBuscar.Clear();
            CargarLista("");
        }
        catch (Exception ex)
        {
            Interfaz.Advertencia(ex.Message);
        }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            Interfaz.Advertencia("Seleccione una editorial de la lista para eliminarla.");
            return;
        }

        if (!Interfaz.Confirmar("¿Está seguro de eliminar la editorial seleccionada?"))
        {
            return;
        }

        try
        {
            negocio.Eliminar(idSeleccionado);
            Interfaz.Informacion("Editorial eliminada correctamente.");
            LimpiarCampos();
            txtBuscar.Clear();
            CargarLista("");
        }
        catch (Exception ex)
        {
            Interfaz.Advertencia(ex.Message);
        }
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
        CargarLista(txtBuscar.Text.Trim());
    }

    private void dgvEditoriales_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewRow fila = dgvEditoriales.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["IdEditorial"].Value);
        txtNombre.Text = Interfaz.Celda(fila, "Nombre");
        txtPais.Text = Interfaz.Celda(fila, "Pais");
    }
}
