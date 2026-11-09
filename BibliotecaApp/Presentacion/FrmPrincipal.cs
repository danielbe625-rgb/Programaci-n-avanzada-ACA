namespace BibliotecaApp.Presentacion;

// Formulario principal: menú lateral y área donde se muestran los demás formularios.
public class FrmPrincipal : Form
{
    private Panel pnlMenu = new Panel();
    private Panel pnlContenido = new Panel();
    private Label lblTitulo = new Label();

    private Button btnInicio;
    private Button btnLibros;
    private Button btnUsuarios;
    private Button btnAutores;
    private Button btnEditoriales;
    private Button btnPrestamos;
    private Button btnReportes;
    private Button btnSalir;

    public FrmPrincipal()
    {
        Text = "Sistema de Gestión de Biblioteca";
        Size = new Size(1180, 720);
        MinimumSize = new Size(1000, 640);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f);

        // Menú lateral
        pnlMenu.Dock = DockStyle.Left;
        pnlMenu.Width = 200;
        pnlMenu.BackColor = Interfaz.ColorPrincipal;

        lblTitulo.Text = "BIBLIOTECA";
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
        lblTitulo.Location = new Point(0, 0);
        lblTitulo.Size = new Size(200, 70);
        lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
        pnlMenu.Controls.Add(lblTitulo);

        btnInicio = CrearBotonMenu("Inicio", 80);
        btnLibros = CrearBotonMenu("Libros", 130);
        btnUsuarios = CrearBotonMenu("Usuarios", 180);
        btnAutores = CrearBotonMenu("Autores", 230);
        btnEditoriales = CrearBotonMenu("Editoriales", 280);
        btnPrestamos = CrearBotonMenu("Préstamos", 330);
        btnReportes = CrearBotonMenu("Reportes", 380);
        btnSalir = CrearBotonMenu("Salir", 460);
        btnSalir.BackColor = Interfaz.ColorRojo;

        btnInicio.Click += btnInicio_Click;
        btnLibros.Click += btnLibros_Click;
        btnUsuarios.Click += btnUsuarios_Click;
        btnAutores.Click += btnAutores_Click;
        btnEditoriales.Click += btnEditoriales_Click;
        btnPrestamos.Click += btnPrestamos_Click;
        btnReportes.Click += btnReportes_Click;
        btnSalir.Click += btnSalir_Click;

        // Área de contenido
        pnlContenido.Dock = DockStyle.Fill;

        Controls.Add(pnlContenido);
        Controls.Add(pnlMenu);

        Load += FrmPrincipal_Load;
    }

    private Button CrearBotonMenu(string texto, int y)
    {
        Button boton = new Button();
        boton.Text = "   " + texto;
        boton.Location = new Point(0, y);
        boton.Size = new Size(200, 46);
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.BackColor = Interfaz.ColorPrincipal;
        boton.ForeColor = Color.White;
        boton.TextAlign = ContentAlignment.MiddleLeft;
        boton.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        boton.Cursor = Cursors.Hand;
        pnlMenu.Controls.Add(boton);
        return boton;
    }

    // Muestra un formulario dentro del área de contenido, reemplazando el anterior
    private void AbrirFormulario(Form formulario, Button botonActivo)
    {
        pnlContenido.Controls.Clear();

        btnInicio.BackColor = Interfaz.ColorPrincipal;
        btnLibros.BackColor = Interfaz.ColorPrincipal;
        btnUsuarios.BackColor = Interfaz.ColorPrincipal;
        btnAutores.BackColor = Interfaz.ColorPrincipal;
        btnEditoriales.BackColor = Interfaz.ColorPrincipal;
        btnPrestamos.BackColor = Interfaz.ColorPrincipal;
        btnReportes.BackColor = Interfaz.ColorPrincipal;
        botonActivo.BackColor = Interfaz.ColorAzul;

        formulario.TopLevel = false;
        formulario.FormBorderStyle = FormBorderStyle.None;
        formulario.Dock = DockStyle.Fill;
        pnlContenido.Controls.Add(formulario);
        formulario.Show();
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmInicio(), btnInicio);
    }

    private void btnInicio_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmInicio(), btnInicio);
    }

    private void btnLibros_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmLibros(), btnLibros);
    }

    private void btnUsuarios_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmUsuarios(), btnUsuarios);
    }

    private void btnAutores_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmAutores(), btnAutores);
    }

    private void btnEditoriales_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmEditoriales(), btnEditoriales);
    }

    private void btnPrestamos_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmPrestamos(), btnPrestamos);
    }

    private void btnReportes_Click(object sender, EventArgs e)
    {
        AbrirFormulario(new FrmReportes(), btnReportes);
    }

    private void btnSalir_Click(object sender, EventArgs e)
    {
        if (Interfaz.Confirmar("¿Desea cerrar la aplicación?"))
        {
            Close();
        }
    }
}
