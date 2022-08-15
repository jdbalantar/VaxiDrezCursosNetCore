using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData> {}

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="userManager"></param>
            /// <param name="jwtGenerador"></param>
            /// <param name="usuarioSesion"></param>
            public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _usuarioSesion = usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                // Buscamos a un usuario en la BD con ese UserName y lo guardamos en una variable
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Token = _jwtGenerador.CrearToken(usuario),
                    Imagen = null
                };

            }
        }
    }
}
