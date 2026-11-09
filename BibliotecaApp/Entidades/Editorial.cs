namespace BibliotecaApp.Entidades;

public class Editorial
{
    public int IdEditorial { get; set; }
    public string Nombre { get; set; }
    public string Pais { get; set; }

    public Editorial() { }

    public Editorial(int idEditorial, string nombre, string pais)
    {
        IdEditorial = idEditorial;
        Nombre = nombre;
        Pais = pais;
    }
}
