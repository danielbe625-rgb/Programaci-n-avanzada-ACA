using System.Data;
using BibliotecaApp.Entidades;
using BibliotecaApp.Negocio;

namespace BibliotecaApp.Presentacion;

// Gestión de préstamos y devoluciones
public class FrmPrestamos : Form
{
    private PrestamoNegocio negocio = new PrestamoNegocio();
    private UsuarioNegocio usuarios = new UsuarioNegocio();
    private LibroNegocio libros = new LibroNegocio();
    private int idSeleccionado = 0;   // 0 = préstamo nuevo

    private Panel pnlDatos = new Panel();
    private DataGridView dgvPrestamos = new DataGridView();
    private ComboBox cmbUsuario;
    private ComboBox cmbLibro;
    private TextBox txtEstado;
    private DateTimePicker dtpFechaPrestamo;
    private DateTimePicker dtpFechaDevolucion;
    private ComboBox cmbFiltroEstado;
    private TextBox txtBuscar;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnDevolver;
    private Button btnEliminar;
    private Button btnBuscar;

    public FrmPrestamos()
    {
        Text = "Préstamos";
        BackColor = Interfaz.ColorFondo;

        pnlDatos.Dock = DockStyle.Top;
        pnlDatos.Height = 230;

        cmbUsuario = Interfaz.CrearCombo(150, 20, 300);
        cmbLibro = Interfaz.CrearCombo(150, 55, 300);
        txtEstado = Interfaz.CrearCaja(150, 90, 150, 10);
        txtEstado.ReadOnly = true;
        dtpFechaPrestamo = Interfaz.CrearFecha(630, 20);
        dtpFechaDevolucion = Interfaz.CrearFecha(630, 55);

        btnNuevo = Interfaz.CrearBoton("Nuevo", 20, 135, 110, Interfaz.ColorAzul);
        btnGuardar = Interfaz.CrearBoton("Guardar", 140, 135, 110, Interfaz.ColorVerde);
        btnDevolver = Interfaz.CrearBoton("Devolver libro", 260, 135, 140, Interfaz.ColorAzul);
        btnEliminar = Interfaz.CrearBoton("Eliminar", 410, 135, 110, Interfaz.ColorRojo);

        txtBuscar = Interfaz.CrearCaja(150, 185, 220, 100);
        cmbFiltroEstado = Interfaz.CrearCombo(470, 185, 120);
        cmbFiltroEstado.Items.Add("Todos");
        cmbFiltroEstado.Items.Add(Prestamo.Prestado);
        cmbFiltroEstado.Items.Add(Prestamo.Devuelto);
        cmbFiltroEstado.SelectedIndex = 0;
        btnBuscar = Interfaz.CrearBoton("Buscar", 610, 181, 90, Interfaz.ColorPrincipal);

        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Usuario:", 20, 24));
        pnlDatos.Controls.Add(cmbUsuario);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Libro:", 20, 59));
        pnlDatos.Controls.Add(cmbLibro);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Estado:", 20, 94));
        pnlDatos.Controls.Add(txtEstado);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Fecha de préstamo:", 490, 24));
        pnlDatos.Controls.Add(dtpFechaPrestamo);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Fecha de devolución:", 490, 59));
        pnlDatos.Controls.Add(dtpFechaDevolucion);
        pnlDatos.Controls.Add(btnNuevo);
        pnlDatos.Controls.Add(btnGuardar);
        pnlDatos.Controls.Add(btnDevolver);
        pnlDatos.Controls.Add(btnEliminar);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Buscar:", 20, 189));
        pnlDatos.Controls.Add(txtBuscar);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Estado:", 400, 189));
        pnlDatos.Controls.Add(cmbFiltroEstado);
        pnlDatos.Controls.Add(btnBuscar);

        Interfaz.ConfigurarGrilla(dgvPrestamos);

        Controls.Add(dgvPrestamos);
        Controls.Add(pnlDatos);

        btnNuevo.Click += btnNuevo_Click;
        btnGuardar.Click += btnGuardar_Click;
        btnDevolver.Click += btnDevolver_Click;
        btnEliminar.Click += btnEliminar_Click;
        btnBuscar.Click += btnBuscar_Click;
        dgvPrestamos.CellClick += dgvPrestamos_CellClick;
        Load += FrmPrestamos_Load;
    }

    private void FrmPrestamos_Load(object sender, EventArgs e)
    {
        CargarCombos();
        CargarLista();
    }

    // Llena los combos de usuarios y libros (los libros muestran sus existencias actuales)
    private void CargarCombos()
    {
        try
        {
            Interfaz.LlenarCombo(cmbUsuario, usuarios.ListarParaCombo(), "NombreCompleto", "IdUsuario");
            Interfaz.LlenarCombo(cmbLibro, libros.ListarParaCombo(), "Descripcion", "IdLibro");
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudieron cargar los usuarios y libros: " + ex.Message);
        }
    }

    // Carga la grilla usando el texto de búsqueda y el estado elegido
    private void CargarLista()
    {
        try
        {
            string estado = "";
            if (cmbFiltroEstado.SelectedIndex > 0)
            {
                estado = cmbFiltroEstado.Text;
            }

            dgvPrestamos.DataSource = negocio.Listar(txtBuscar.Text.Trim(), estado);
            dgvPrestamos.Columns["IdUsuario"].Visible = false;
            dgvPrestamos.Columns["IdLibro"].Visible = false;
            dgvPrestamos.Columns["IdPrestamo"].HeaderText = "N°";
            dgvPrestamos.Columns["FechaPrestamo"].HeaderText = "Fecha de préstamo";
            dgvPrestamos.Columns["FechaDevolucion"].HeaderText = "Fecha de devolución";
            dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvPrestamos.Columns["FechaDevolucion"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvPrestamos.ClearSelection();
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudo cargar la lista de préstamos: " + ex.Message);
        }
    }

    private void LimpiarCampos()
    {
        idSeleccionado = 0;
        cmbUsuario.SelectedIndex = -1;
        cmbLibro.SelectedIndex = -1;
        cmbLibro.Enabled = true;
        txtEstado.Clear();
        dtpFechaPrestamo.Value = DateTime.Today;
        dtpFechaDevolucion.Value = DateTime.Today;
        dgvPrestamos.ClearSelection();
    }

    // Después de guardar, devolver o eliminar se actualiza todo
    private void ActualizarPantalla()
    {
        LimpiarCampos();
        txtBuscar.Clear();
        CargarCombos();
        CargarLista();
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            int idUsuario = Interfaz.ValorCombo(cmbUsuario);

            if (idSeleccionado == 0)
            {
                // Préstamo nuevo
                int idLibro = Interfaz.ValorCombo(cmbLibro);
                Prestamo prestamo = new Prestamo(idUsuario, idLibro, dtpFechaPrestamo.Value.Date);
                negocio.Registrar(prestamo);
                Interfaz.Informacion("Préstamo registrado correctamente. Se descontó una existencia del libro.");
            }
            else
            {
                // Préstamo existente: solo se puede cambiar el usuario y la fecha
                negocio.Actualizar(idSeleccionado, idUsuario, dtpFechaPrestamo.Value.Date);
                Interfaz.Informacion("Préstamo actualizado correctamente.");
            }

            ActualizarPantalla();
        }
        catch (Exception ex)
        {
            Interfaz.Advertencia(ex.Message);
        }
    }

    private void btnDevolver_Click(object sender, EventArgs e)
    {
        if (idSeleccionado == 0)
        {
            Interfaz.Advertencia("Seleccione de la lista el préstamo que desea devolver.");
            return;
        }

        string pregunta = "¿Registrar la devolución con fecha " + dtpFechaDevolucion.Value.ToString("dd/MM/yyyy") + "?";
        if (!Interfaz.Confirmar(pregunta))
        {
            return;
        }

        try
        {
            negocio.Devolver(idSeleccionado, dtpFechaDevolucion.Value.Date);
            Interfaz.Informacion("Devolución registrada correctamente. Se actualizó la existencia del libro.");
            ActualizarPantalla();
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
            Interfaz.Advertencia("Seleccione un préstamo de la lista para eliminarlo.");
            return;
        }

        if (!Interfaz.Confirmar("¿Está seguro de eliminar el préstamo seleccionado?"))
        {
            return;
        }

        try
        {
            negocio.Eliminar(idSeleccionado);
            Interfaz.Informacion("Préstamo eliminado correctamente.");
            ActualizarPantalla();
        }
        catch (Exception ex)
        {
            Interfaz.Advertencia(ex.Message);
        }
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
        CargarLista();
    }

    private void dgvPrestamos_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewRow fila = dgvPrestamos.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["IdPrestamo"].Value);
        cmbUsuario.SelectedValue = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
        cmbLibro.SelectedValue = Convert.ToInt32(fila.Cells["IdLibro"].Value);
        cmbLibro.Enabled = false;   // el libro de un préstamo ya registrado no se cambia
        txtEstado.Text = Interfaz.Celda(fila, "Estado");
        dtpFechaPrestamo.Value = Convert.ToDateTime(fila.Cells["FechaPrestamo"].Value);

        if (fila.Cells["FechaDevolucion"].Value == DBNull.Value)
            dtpFechaDevolucion.Value = DateTime.Today;
        else
            dtpFechaDevolucion.Value = Convert.ToDateTime(fila.Cells["FechaDevolucion"].Value);
    }
}
