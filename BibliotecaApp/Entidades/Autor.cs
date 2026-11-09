namespace BibliotecaApp.Entidades;

public class Autor : Persona
{
    public int IdAutor { get; set; }

    public Autor() { }

    public Autor(int idAutor, string nombre, string apellido) : base(nombre, apellido)
    {
        IdAutor = idAutor;
    }
}
