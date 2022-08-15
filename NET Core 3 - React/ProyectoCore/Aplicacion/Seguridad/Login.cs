using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _jwtGenerador;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="userManager">Este es el "contexto" pero de los usuarios. Tiene la inforamción de todos los usuarios de Net Core Identity</param>
            /// <param name="signInManager">Objeto para iniciar sesión</param>
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
            }

            /// <summary>
            /// Método que implementa la lógica de inicio de sesión
            /// </summary>
            /// <param name="request">Representa la información que entra por el Controller</param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="ManejadorExcepcion"></exception>
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Vamos a buscar los usuarios por el ID que llega del Controller y lo añadimos a la variable usuario
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    // Si el usuario no existe, lanzamos una excepción (No se encontró, desautorizado, etc)
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new { usuario = "Este usuario no tiene permisos para ingresar a la aplicación" });
                }

                // Si se encontró al usuario, intente iniciar sesión con el usuario que llegó, y le pasamos la contraseña que llegó por el controller
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                // Si el inicio de sesión fue exitoso, devolvemos un DTO de tipo UsuarioData, para no devolverle todos los datos del usuario, sino unos cuantos datos, por seguridad
                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        UserName = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Imagen = null
                    };
                }

                // Si el inicio de sesión no fue exitoso (En este caso, puede ser porque la contraseña es incorrecta), entonces, lanzamos una excepción
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}
