namespace BibliotecaApp.Entidades;

public class Usuario : Persona
{
    public int IdUsuario { get; set; }
    public string Documento { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }

    public Usuario() { }

    public Usuario(int idUsuario, string nombre, string apellido, string documento, string telefono, string correo)
        : base(nombre, apellido)
    {
        IdUsuario = idUsuario;
        Documento = documento;
        Telefono = telefono;
        Correo = correo;
    }
}
