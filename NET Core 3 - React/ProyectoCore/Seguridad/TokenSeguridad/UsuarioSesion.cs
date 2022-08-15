using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        // Para tener acceso al usuario que está en sesión se debe usar el IHttpContextAccessor

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string ObtenerUsuarioSesion()
        {
            // Creamos una variable donde se almacenará el usuario. Validamo si es Nulo '?'
            var userName =
                _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}
