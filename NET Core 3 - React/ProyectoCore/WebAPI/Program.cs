using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostServer = CreateHostBuilder(args).Build();
            using (var ambiente = hostServer.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;
                try
                {
                    // Importamos el UserManager
                    var userManager = services.GetRequiredService<UserManager<Usuario>>();
                    var context = services.GetRequiredService<CursosOnlineContext>();
                    context.Database.Migrate();
                    // Invocamos el metodo para crear usuarios automaticamente
                    DataPrueba.InsertarData(context, userManager).Wait();
                }
                catch (Exception e)
                {
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(e, "Ocurrió un error en la migración");
                }
            }
            hostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
