using System.Data;
using BibliotecaApp.Entidades;
using BibliotecaApp.Negocio;

namespace BibliotecaApp.Presentacion;

public class FrmLibros : Form
{
    private LibroNegocio negocio = new LibroNegocio();
    private AutorNegocio autores = new AutorNegocio();
    private EditorialNegocio editoriales = new EditorialNegocio();
    private int idSeleccionado = 0;   // 0 = libro nuevo

    private Panel pnlDatos = new Panel();
    private DataGridView dgvLibros = new DataGridView();
    private TextBox txtCodigo;
    private TextBox txtTitulo;
    private TextBox txtAnio;
    private ComboBox cmbAutor;
    private ComboBox cmbEditorial;
    private NumericUpDown nudExistencias = new NumericUpDown();
    private TextBox txtBuscar;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
    private Button btnBuscar;

    public FrmLibros()
    {
        Text = "Libros";
        BackColor = Interfaz.ColorFondo;

        pnlDatos.Dock = DockStyle.Top;
        pnlDatos.Height = 230;

        txtCodigo = Interfaz.CrearCaja(150, 20, 220, 20);
        txtTitulo = Interfaz.CrearCaja(150, 55, 220, 150);
        txtAnio = Interfaz.CrearCaja(150, 90, 100, 4);
        cmbAutor = Interfaz.CrearCombo(550, 20, 240);
        cmbEditorial = Interfaz.CrearCombo(550, 55, 240);

        nudExistencias.Location = new Point(550, 90);
        nudExistencias.Width = 100;
        nudExistencias.Minimum = 0;
        nudExistencias.Maximum = 9999;
        nudExistencias.Value = 1;

        btnNuevo = Interfaz.CrearBoton("Nuevo", 20, 135, 110, Interfaz.ColorAzul);
        btnGuardar = Interfaz.CrearBoton("Guardar", 140, 135, 110, Interfaz.ColorVerde);
        btnEliminar = Interfaz.CrearBoton("Eliminar", 260, 135, 110, Interfaz.ColorRojo);
        txtBuscar = Interfaz.CrearCaja(150, 185, 220, 100);
        btnBuscar = Interfaz.CrearBoton("Buscar", 380, 181, 90, Interfaz.ColorPrincipal);

        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Código / ISBN:", 20, 24));
        pnlDatos.Controls.Add(txtCodigo);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Título:", 20, 59));
        pnlDatos.Controls.Add(txtTitulo);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Año de publicación:", 20, 94));
        pnlDatos.Controls.Add(txtAnio);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Autor:", 440, 24));
        pnlDatos.Controls.Add(cmbAutor);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Editorial:", 440, 59));
        pnlDatos.Controls.Add(cmbEditorial);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Existencias:", 440, 94));
        pnlDatos.Controls.Add(nudExistencias);
        pnlDatos.Controls.Add(btnNuevo);
        pnlDatos.Controls.Add(btnGuardar);
        pnlDatos.Controls.Add(btnEliminar);
        pnlDatos.Controls.Add(Interfaz.CrearEtiqueta("Buscar:", 20, 189));
        pnlDatos.Controls.Add(txtBuscar);
        pnlDatos.Controls.Add(btnBuscar);

        Interfaz.ConfigurarGrilla(dgvLibros);

        Controls.Add(dgvLibros);
        Controls.Add(pnlDatos);

        btnNuevo.Click += btnNuevo_Click;
        btnGuardar.Click += btnGuardar_Click;
        btnEliminar.Click += btnEliminar_Click;
        btnBuscar.Click += btnBuscar_Click;
        dgvLibros.CellClick += dgvLibros_CellClick;
        Load += FrmLibros_Load;
    }

    private void FrmLibros_Load(object sender, EventArgs e)
    {
        try
        {
            Interfaz.LlenarCombo(cmbAutor, autores.ListarParaCombo(), "NombreCompleto", "IdAutor");
            Interfaz.LlenarCombo(cmbEditorial, editoriales.Listar(""), "Nombre", "IdEditorial");
            CargarLista("");
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudieron cargar los datos: " + ex.Message);
        }
    }

    private void CargarLista(string filtro)
    {
        try
        {
            dgvLibros.DataSource = negocio.Listar(filtro);
            dgvLibros.Columns["IdLibro"].Visible = false;
            dgvLibros.Columns["IdAutor"].Visible = false;
            dgvLibros.Columns["IdEditorial"].Visible = false;
            dgvLibros.Columns["Codigo"].HeaderText = "Código";
            dgvLibros.Columns["Titulo"].HeaderText = "Título";
            dgvLibros.Columns["Anio"].HeaderText = "Año";
            dgvLibros.ClearSelection();
        }
        catch (Exception ex)
        {
            Interfaz.Error("No se pudo cargar la lista de libros: " + ex.Message);
        }
    }

    private void LimpiarCampos()
    {
        idSeleccionado = 0;
        txtCodigo.Clear();
        txtTitulo.Clear();
        txtAnio.Clear();
        cmbAutor.SelectedIndex = -1;
        cmbEditorial.SelectedIndex = -1;
        nudExistencias.Value = 1;
        txtCodigo.Focus();
        dgvLibros.ClearSelection();
    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            // El año es opcional; si se escribe, debe ser un número entero
            int? anio = null;
            if (txtAnio.Text.Trim() != "")
            {
                int valorAnio;
                if (!int.TryParse(txtAnio.Text.Trim(), out valorAnio))
                {
                    throw new Exception("El año de publicación debe ser un número entero.");
                }
                anio = valorAnio;
            }

            bool esNuevo = (idSeleccionado == 0);
            Libro libro = new Libro(idSeleccionado, txtCodigo.Text.Trim(), txtTitulo.Text.Trim(),
                                    Interfaz.ValorCombo(cmbAutor), Interfaz.ValorCombo(cmbEditorial),
                                    anio, (int)nudExistencias.Value);
            negocio.Guardar(libro);

            if (esNuevo)
                Interfaz.Informacion("Libro registrado correctamente.");
            else
                Interfaz.Informacion("Libro actualizado correctamente.");

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
            Interfaz.Advertencia("Seleccione un libro de la lista para eliminarlo.");
            return;
        }

        if (!Interfaz.Confirmar("¿Está seguro de eliminar el libro seleccionado?"))
        {
            return;
        }

        try
        {
            negocio.Eliminar(idSeleccionado);
            Interfaz.Informacion("Libro eliminado correctamente.");
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

    private void dgvLibros_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];
        idSeleccionado = Convert.ToInt32(fila.Cells["IdLibro"].Value);
        txtCodigo.Text = Interfaz.Celda(fila, "Codigo");
        txtTitulo.Text = Interfaz.Celda(fila, "Titulo");
        txtAnio.Text = Interfaz.Celda(fila, "Anio");
        cmbAutor.SelectedValue = Convert.ToInt32(fila.Cells["IdAutor"].Value);
        cmbEditorial.SelectedValue = Convert.ToInt32(fila.Cells["IdEditorial"].Value);
        nudExistencias.Value = Convert.ToInt32(fila.Cells["Existencias"].Value);
    }
}
