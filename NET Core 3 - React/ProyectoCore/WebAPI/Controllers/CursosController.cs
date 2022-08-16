using System;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class CursosController : MiControllerBase
    {

        /// <summary>
        /// Método que devuelve una lista de cursos
        /// El método Send envia un request a la clase Consulta
        /// dentro de Send instanciamos la clase Consulta y ejecutamos el método @ListaCursos
        /// </summary>
        /// <returns>Lista de cursos</returns>
        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> Lista()
        {
            return await Mediator.Send(new Consulta.ListaCursos());
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<CursoDto>> Detalle(Guid id)
        {

            return await Mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta { Id = id });
        }
    }
}
