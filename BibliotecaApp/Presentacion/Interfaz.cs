using System.Data;

namespace BibliotecaApp.Presentacion;

// Métodos de ayuda para crear controles y mostrar mensajes.
// Así cada formulario queda más corto y fácil de leer.
public static class Interfaz
{
    public static Color ColorPrincipal = Color.FromArgb(31, 56, 100);
    public static Color ColorAzul = Color.FromArgb(46, 117, 182);
    public static Color ColorVerde = Color.FromArgb(39, 138, 87);
    public static Color ColorRojo = Color.FromArgb(192, 57, 43);
    public static Color ColorFondo = Color.FromArgb(244, 246, 250);

    // ---------- Creación de controles ----------

    public static Label CrearEtiqueta(string texto, int x, int y)
    {
        Label etiqueta = new Label();
        etiqueta.Text = texto;
        etiqueta.Location = new Point(x, y);
        etiqueta.AutoSize = true;
        etiqueta.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        etiqueta.ForeColor = ColorPrincipal;
        return etiqueta;
    }

    public static TextBox CrearCaja(int x, int y, int ancho, int longitudMaxima)
    {
        TextBox caja = new TextBox();
        caja.Location = new Point(x, y);
        caja.Width = ancho;
        caja.MaxLength = longitudMaxima;
        return caja;
    }

    public static ComboBox CrearCombo(int x, int y, int ancho)
    {
        ComboBox combo = new ComboBox();
        combo.Location = new Point(x, y);
        combo.Width = ancho;
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        return combo;
    }

    public static DateTimePicker CrearFecha(int x, int y)
    {
        DateTimePicker fecha = new DateTimePicker();
        fecha.Location = new Point(x, y);
        fecha.Width = 130;
        fecha.Format = DateTimePickerFormat.Custom;
        fecha.CustomFormat = "dd/MM/yyyy";
        return fecha;
    }

    public static Button CrearBoton(string texto, int x, int y, int ancho, Color color)
    {
        Button boton = new Button();
        boton.Text = texto;
        boton.Location = new Point(x, y);
        boton.Size = new Size(ancho, 34);
        boton.BackColor = color;
        boton.ForeColor = Color.White;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        boton.Cursor = Cursors.Hand;
        return boton;
    }

    public static void ConfigurarGrilla(DataGridView grilla)
    {
        grilla.Dock = DockStyle.Fill;
        grilla.ReadOnly = true;
        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.MultiSelect = false;
        grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grilla.RowHeadersVisible = false;
        grilla.BackgroundColor = Color.White;
        grilla.EnableHeadersVisualStyles = false;
        grilla.ColumnHeadersDefaultCellStyle.BackColor = ColorPrincipal;
        grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        grilla.DefaultCellStyle.SelectionBackColor = ColorAzul;
        grilla.DefaultCellStyle.SelectionForeColor = Color.White;
    }

    // ---------- Utilidades ----------

    // Llena un ComboBox con los datos de una tabla
    public static void LlenarCombo(ComboBox combo, DataTable tabla, string campoMostrar, string campoValor)
    {
        combo.DataSource = tabla;
        combo.DisplayMember = campoMostrar;
        combo.ValueMember = campoValor;
        combo.SelectedIndex = -1;
    }

    // Devuelve el valor seleccionado en un ComboBox (0 si no hay nada seleccionado)
    public static int ValorCombo(ComboBox combo)
    {
        if (combo.SelectedIndex == -1)
        {
            return 0;
        }
        return Convert.ToInt32(combo.SelectedValue);
    }

    // Devuelve el texto de una celda ("" si está vacía)
    public static string Celda(DataGridViewRow fila, string columna)
    {
        object valor = fila.Cells[columna].Value;
        if (valor == null || valor == DBNull.Value)
        {
            return "";
        }
        return valor.ToString();
    }

    // ---------- Mensajes ----------

    public static void Informacion(string mensaje)
    {
        MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void Advertencia(string mensaje)
    {
        MessageBox.Show(mensaje, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static void Error(string mensaje)
    {
        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static bool Confirmar(string mensaje)
    {
        DialogResult respuesta = MessageBox.Show(mensaje, "Confirmación",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return respuesta == DialogResult.Yes;
    }
}
