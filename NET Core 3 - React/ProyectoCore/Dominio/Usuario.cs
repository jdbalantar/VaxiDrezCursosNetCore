using Microsoft.AspNetCore.Identity;

namespace Dominio
{
    /// <summary>
    /// Clase que representará al usuario
    /// </summary>
    public class Usuario : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
