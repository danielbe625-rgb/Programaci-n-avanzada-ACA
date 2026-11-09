namespace BibliotecaApp.Entidades;

public class Prestamo
{
    // Estados posibles de un préstamo
    public const string Prestado = "Prestado";
    public const string Devuelto = "Devuelto";

    public int IdPrestamo { get; set; }
    public int IdUsuario { get; set; }
    public int IdLibro { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }   // vacía mientras el libro no se devuelva
    public string Estado { get; set; }

    public Prestamo()
    {
        FechaPrestamo = DateTime.Today;
        Estado = Prestado;
    }

    public Prestamo(int idUsuario, int idLibro, DateTime fechaPrestamo)
    {
        IdUsuario = idUsuario;
        IdLibro = idLibro;
        FechaPrestamo = fechaPrestamo;
        Estado = Prestado;
    }
}
