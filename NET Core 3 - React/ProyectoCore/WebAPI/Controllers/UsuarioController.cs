using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registar(Registrar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Ejecutar());
        }
    }
}
