namespace BibliotecaApp.Entidades;

// Clase base para las entidades que representan personas (Autor y Usuario).
// Aquí se aplica el encapsulamiento (campos privados) y la herencia.
public class Persona
{
    private string nombre;
    private string apellido;

    public Persona()
    {
        nombre = "";
        apellido = "";
    }

    public Persona(string nombre, string apellido)
    {
        Nombre = nombre;
        Apellido = apellido;
    }

    public string Nombre
    {
        get { return nombre; }
        set { nombre = (value == null) ? "" : value.Trim(); }
    }

    public string Apellido
    {
        get { return apellido; }
        set { apellido = (value == null) ? "" : value.Trim(); }
    }

    public string NombreCompleto()
    {
        return Nombre + " " + Apellido;
    }
}
