using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    /// <summary>
    /// Clase para manejar el Registro del Usuario
    /// </summary>
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }



        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Vamos a buscar en los registros de la tabla Users, buscamos si hay un Email que sea igual al
                // que el usuario está ingresando. Esto devuelve un bool.
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                // Si devuelve true (Si el correo existe) entonces:
                if (existe)
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El email ingresado ya existe" });

                //
                var existeUserName = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();
                if(existeUserName)
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "El usuario ingresado ya existe" });
                
                // Si el correo o usuario no existen, creamos un objeto de tipo entidad Usuario
                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.UserName

                };

                // Intentamos registrar el usuario
                var resultado = await _userManager.CreateAsync(usuario, request.Password);

                // Si el usuario se registró correctamente, devolvemos el usuario en un DTO
                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Email = usuario.Email,
                        UserName = usuario.UserName
                    };
                }
                // Sino...
                throw new Exception("No se pudo agregar al nuevo usuario");
            }
        }
    }
}
