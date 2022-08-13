using Dominio;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia
{
    /// <summary>
    /// Con esta clase voy a insertar un usuario automaticamente en la BD cada vez que se ejecuta el programa. Esto solo en caso de que no haya usuarios
    /// </summary>
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if (!usuarioManager.Users.Any())
            {
                var usuario = new Usuario { NombreCompleto = "David Balanta", UserName = "jdbalantar", Email = "jdbalantar@gmail.com" };
                await usuarioManager.CreateAsync(usuario, "Pa$$W0rd");
            }
        }
    }
}
