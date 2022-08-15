using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistencia;
using Seguridad.TokenSeguridad;
using System.Text;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Añadiendo el contexto a los servicios
            services.AddDbContext<CursosOnlineContext>(opt =>
            {
                opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Añadiendo el patrón MediatR
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            // Añadiendo servicio de Authorizacion para todos los controllers
            services.AddControllers(opt =>
                {
                    // Con esta policy indicamos que el usuario debe estar autenticado para consumir el controller
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    opt.Filters.Add(new AuthorizeFilter(policy));
                });

            // Añadiendo el servicio FluentValidation
            services.AddFluentValidationAutoValidation();
            // Añadiendo el Assembly donde se encuentran las validaciones
            services.AddValidatorsFromAssemblyContaining<Nuevo>();


            // Añadiendo CoreIdentity
            // Añadimos como servicio el IdentityCore, y le indicamos cuál será la clase que manejará el Usuario
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            // A identityBuilder le pasamos el contexto, ya que con FrameworkStores le indicamos cual es la clase que manejará la informacion de los usuarios
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            // Le indicamos que vamos a manejar el Login, y se va a manejar con CoreIdentity, y que los datos los va a tomar de la clase Usuario
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();

            // Añadiendo la autenticacion de tipo Bearer
            // Creamos una variable llamada key y colocamos la palabra clave puesta dentro del Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi Palabra Secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                // Indicaremos los parametros del token
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    // Indica que cualquier tipo de Request de un cliente debe ser validado por la lógica dentro 
                    // del token y pasando del IdentityCore
                    ValidateIssuerSigningKey = true,
                    // Le pasamos la palabra clave que indicamos dentro del proyecto Aplicacion
                    IssuerSigningKey = key,
                    // ¿Quien va a poder crear esos tokens? - 
                    ValidateAudience = false,
                    // Indicamos que aqui el token y validacion se le pueda enviar a cualquier persona
                    ValidateIssuer = false
                };
            });

            // Inyectando el servicio Generador del Token
            services.AddScoped<IJwtGenerador, JwtGenerador>();

            // Añadiendo el servicio para obtener el usuario en sesión
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Usando el Middleware para manejar las excepciones en el servidor
            app.UseMiddleware<ManejadorErrorMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            // Añadimos la autenticacion
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
