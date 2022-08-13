using Aplicacion.Cursos;
using Dominio;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Persistencia;
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

            services.AddControllers();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
