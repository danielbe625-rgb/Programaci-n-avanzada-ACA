using System.Data;
using BibliotecaApp.Entidades;
using BibliotecaApp.Negocio;

namespace BibliotecaApp.Presentacion;

public class FrmAutores : Form
{
    private AutorNegocio negocio = new AutorNegocio();
    private int idSeleccionado = 0;   // 0 = autor nuevo

    private Panel pnlDatos = new Panel();
    private DataGridView dgvAutores = new DataGridView();
    private TextBox txtNombre;
    private TextBox txtApellido;
    private TextBox txtBuscar;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
    private Button btnBuscar;

    public FrmAutores()
    {
        Text = "Autores";
        BackColor = Interfaz.ColorFondo;

        // Panel superior con los datos
        pnlDatos.Dock = DockStyle.Top;
        pnlDatos.Height = 150;

        txtNombre = Interfaz.CrearCaja(130, 20, 220, 60);
        txtApellido = Interfaz.CrearCaja(520, 20, 220, 60);
        btnNuevo = Interfaz.CrearBoton("Nuevo", 20, 65, 110, Interfaz.ColorAzul);
        btnGuardar = Interfaz.CrearBoton("Guardar", 140, 65, 110, Interfaz.ColorVerde);
        btnEliminar = Interfaz.CrearBoton("Eliminar", 260, 65, 110, Interfaz.ColorRojo);
        txtBuscar = Interfaz.CrearCaja(130, 110, 220, 100);
        btnBuscar = Interfaz.CrearBoton("Buscar", 360, 106, 90, Interfaz.ColorPrincipal);

        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Nombre:", 20, 24));
        pnlDatos.Controls.Add(txtNombre);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Apellido:", 410, 24));
        pnlDatos.Controls.Add(txtApellido);
        pnlDatos.Controls.Add(btnNuevo);
        pnlDatos.Controls.Add(btnGuardar);
        pnlDatos.Controls.Add(btnEliminar);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Buscar:", 20, 114));
        pnlDatos.Controls.Add(txtBuscar);
        pnlDatos.Controls.Add(btnBuscar);

        // Grilla
        Interfaz.ConfigurarGrilla(dgvAutores);

        // Primero la grilla (Fill) y después el panel superior
        Controls.Add(dgvAutores);
        Controls.Add(pnlDatos);

        // Eventos
        btnNuevo.Click += btnNuevo_Click;
        btnGuardar.Click += btnGuardar_Click;
        btnEliminar.Click += btnEliminar_Click;
        btnBuscar.Click += btnBuscar_Click;
        dgvAutores.CellClick += dgvAutores_CellClick;
        Load += FrmAutores_Load;
    }

    private void FrmAutores_Load(object sender, EventArgs e)
    {
        CargarLista("");
    }

    private void CargarLista(string filtro)
    {
        try
        {
            dgvAutores.DataSource = negocio.Listar(filtro);
            dgvAutores.Columns["IdAutor"].Visible = false;
            dgvAutores.ClearSelection();
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudo cargar la lista de autores: " + ex.Message);
        }
    }

    private void LimpiarCampos()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        txtApellido.Clear();
        txtNombre.Focus();
        dgvAutores.ClearSelection();
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
            Autor autor = new Autor(idSeleccionado, txtNombre.Text, txtApellido.Text);
            negocio.Guardar(autor);

            if (esNuevo)
                Interfaz.Informacion("Autor registrado correctamente.");
            else
                Interfaz.Informacion("Autor actualizado correctamente.");

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
            Interfaz.Advertencia("Seleccione un autor de la lista para eliminarlo.");
            return;
        }

        if (!Interfaz.Confirmar("¿Está seguro de eliminar el autor seleccionado?"))
        {
            return;
        }

        try
        {
            negocio.Eliminar(idSeleccionado);
            Interfaz.Informacion("Autor eliminado correctamente.");
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

    // Al hacer clic en una fila, sus datos pasan a los cuadros de texto
    private void dgvAutores_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewRow fila = dgvAutores.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["IdAutor"].Value);
        txtNombre.Text = Interfaz.Celda(fila, "Nombre");
        txtApellido.Text = Interfaz.Celda(fila, "Apellido");
    }
}
