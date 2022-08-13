namespace Aplicacion.Seguridad
{
    /// <summary>
    /// Esta clase representa la información que se le devolverá al cliente al iniciar sesión.
    /// Usaremos esta clase como "DTO" para que no haya necesidad de darle toda la informacion del usuario al iniciar sesión,
    /// y evitar exponer los datos confidenciales en la API
    /// </summary>
    public class UsuarioData
    {
        public string NombreCompleto { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Imagen { get; set; }
    }
}
