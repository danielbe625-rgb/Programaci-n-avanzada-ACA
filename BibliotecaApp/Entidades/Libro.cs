namespace BibliotecaApp.Entidades;

public class Libro
{
    public int IdLibro { get; set; }
    public string Codigo { get; set; }
    public string Titulo { get; set; }
    public int IdAutor { get; set; }
    public int IdEditorial { get; set; }
    public int? AnioPublicacion { get; set; }   // es opcional
    public int Existencias { get; set; }

    public Libro() { }

    public Libro(int idLibro, string codigo, string titulo, int idAutor, int idEditorial,
                 int? anioPublicacion, int existencias)
    {
        IdLibro = idLibro;
        Codigo = codigo;
        Titulo = titulo;
        IdAutor = idAutor;
        IdEditorial = idEditorial;
        AnioPublicacion = anioPublicacion;
        Existencias = existencias;
    }
}
