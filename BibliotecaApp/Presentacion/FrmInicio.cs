namespace BibliotecaApp.Presentacion;

// Pantalla de bienvenida
public class FrmInicio : Form
{
    public FrmInicio()
    {
        Text = "Inicio";
        BackColor = Interfaz.ColorFondo;

        Label lblBienvenida = new Label();
        lblBienvenida.Text = "Bienvenido al Sistema de Gestión de Biblioteca";
        lblBienvenida.Font = new Font("Segoe UI", 22f, FontStyle.Bold);
        lblBienvenida.ForeColor = Interfaz.ColorPrincipal;
        lblBienvenida.AutoSize = true;
        lblBienvenida.Location = new Point(40, 60);

        Label lblDescripcion = new Label();
        lblDescripcion.Text = "Use el menú de la izquierda para administrar los libros, usuarios, autores,\n" +
                              "editoriales y préstamos, o para consultar los reportes de la biblioteca.";
        lblDescripcion.Font = new Font("Segoe UI", 12f);
        lblDescripcion.ForeColor = Color.DimGray;
        lblDescripcion.AutoSize = true;
        lblDescripcion.Location = new Point(44, 130);

        Controls.Add(lblBienvenida);
        Controls.Add(lblDescripcion);
    }
}
