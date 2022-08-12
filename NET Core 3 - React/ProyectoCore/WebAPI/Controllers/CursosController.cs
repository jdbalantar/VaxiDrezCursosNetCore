using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Routing;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CursosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Método que devuelve una lista de cursos
        /// El método Send envia un request a la clase Consulta
        /// dentro de Send instanciamos la clase Consulta y ejecutamos el método @ListaCursos
        /// </summary>
        /// <returns>Lista de cursos</returns>
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Lista()
        {
            return await _mediator.Send(new Consulta.ListaCursos());
        }


        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Curso>> Curso(int id){
        
            return await _mediator.Send(new ConsultaId.CursoUnico{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult<Unit>> Editar(int id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await _mediator.Send(data);
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<Unit>> Eliminar(int id)
        {
            return await _mediator.Send(new Eliminar.Ejecuta{Id = id});
        }
    }
}
