using System.Data;
using BibliotecaApp.Entidades;
using BibliotecaApp.Negocio;

namespace BibliotecaApp.Presentacion;

public class FrmUsuarios : Form
{
    private UsuarioNegocio negocio = new UsuarioNegocio();
    private int idSeleccionado = 0;   // 0 = usuario nuevo

    private Panel pnlDatos = new Panel();
    private DataGridView dgvUsuarios = new DataGridView();
    private TextBox txtDocumento;
    private TextBox txtNombre;
    private TextBox txtApellido;
    private TextBox txtTelefono;
    private TextBox txtCorreo;
    private TextBox txtBuscar;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
    private Button btnBuscar;

    public FrmUsuarios()
    {
        Text = "Usuarios";
        BackColor = Interfaz.ColorFondo;

        pnlDatos.Dock = DockStyle.Top;
        pnlDatos.Height = 230;

        txtDocumento = Interfaz.CrearCaja(130, 20, 220, 20);
        txtNombre = Interfaz.CrearCaja(130, 55, 220, 60);
        txtApellido = Interfaz.CrearCaja(130, 90, 220, 60);
        txtTelefono = Interfaz.CrearCaja(550, 20, 220, 20);
        txtCorreo = Interfaz.CrearCaja(550, 55, 220, 100);
        btnNuevo = Interfaz.CrearBoton("Nuevo", 20, 135, 110, Interfaz.ColorAzul);
        btnGuardar = Interfaz.CrearBoton("Guardar", 140, 135, 110, Interfaz.ColorVerde);
        btnEliminar = Interfaz.CrearBoton("Eliminar", 260, 135, 110, Interfaz.ColorRojo);
        txtBuscar = Interfaz.CrearCaja(130, 185, 220, 100);
        btnBuscar = Interfaz.CrearBoton("Buscar", 360, 181, 90, Interfaz.ColorPrincipal);

        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Documento:", 20, 24));
        pnlDatos.Controls.Add(txtDocumento);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Nombre:", 20, 59));
        pnlDatos.Controls.Add(txtNombre);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Apellido:", 20, 94));
        pnlDatos.Controls.Add(txtApellido);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Teléfono:", 410, 24));
        pnlDatos.Controls.Add(txtTelefono);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Correo electrónico:", 410, 59));
        pnlDatos.Controls.Add(txtCorreo);
        pnlDatos.Controls.Add(btnNuevo);
        pnlDatos.Controls.Add(btnGuardar);
        pnlDatos.Controls.Add(btnEliminar);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Buscar:", 20, 189));
        pnlDatos.Controls.Add(txtBuscar);
        pnlDatos.Controls.Add(btnBuscar);

        Interfaz.ConfigurarGrilla(dgvUsuarios);

        Controls.Add(dgvUsuarios);
        Controls.Add(pnlDatos);

        btnNuevo.Click += btnNuevo_Click;
        btnGuardar.Click += btnGuardar_Click;
        btnEliminar.Click += btnEliminar_Click;
        btnBuscar.Click += btnBuscar_Click;
        dgvUsuarios.CellClick += dgvUsuarios_CellClick;
        Load += FrmUsuarios_Load;
    }

    private void FrmUsuarios_Load(object sender, EventArgs e)
    {
        CargarLista("");
    }

    private void CargarLista(string filtro)
    {
        try
        {
            dgvUsuarios.DataSource = negocio.Listar(filtro);
            dgvUsuarios.Columns["IdUsuario"].Visible = false;
            dgvUsuarios.Columns["Telefono"].HeaderText = "Teléfono";
            dgvUsuarios.Columns["Correo"].HeaderText = "Correo electrónico";
            dgvUsuarios.ClearSelection();
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudo cargar la lista de usuarios: " + ex.Message);
        }
    }

    private void LimpiarCampos()
    {
        idSeleccionado = 0;
        txtDocumento.Clear();
        txtNombre.Clear();
        txtApellido.Clear();
        txtTelefono.Clear();
        txtCorreo.Clear();
        txtDocumento.Focus();
        dgvUsuarios.ClearSelection();
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
            Usuario usuario = new Usuario(idSeleccionado, txtNombre.Text, txtApellido.Text,
                                          txtDocumento.Text.Trim(), txtTelefono.Text.Trim(), txtCorreo.Text.Trim());
            negocio.Guardar(usuario);

            if (esNuevo)
                Interfaz.Informacion("Usuario registrado correctamente.");
            else
                Interfaz.Informacion("Usuario actualizado correctamente.");

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
            Interfaz.Advertencia("Seleccione un usuario de la lista para eliminarlo.");
            return;
        }

        if (!Interfaz.Confirmar("¿Está seguro de eliminar el usuario seleccionado?"))
        {
            return;
        }

        try
        {
            negocio.Eliminar(idSeleccionado);
            Interfaz.Informacion("Usuario eliminado correctamente.");
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

    private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
        txtDocumento.Text = Interfaz.Celda(fila, "Documento");
        txtNombre.Text = Interfaz.Celda(fila, "Nombre");
        txtApellido.Text = Interfaz.Celda(fila, "Apellido");
        txtTelefono.Text = Interfaz.Celda(fila, "Telefono");
        txtCorreo.Text = Interfaz.Celda(fila, "Correo");
    }
}
