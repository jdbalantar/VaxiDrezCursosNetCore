using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructores { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            /// <summary>
            /// En este constructor van las reglas de validación.
            /// </summary>
            public EjecutaValidacion()
            {
                // Con esto hago que sea obligatorio
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                    // Haciendo uso de la Excepción ManejadorExcepcion, para cuando haya un error en el API
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontró el curso" });

                // Si no le estoy enviando nada, no actualice, déjelo igual
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                // Los DateTime no permiten NULL por defecto
                // Por eso, en la clase Ejecuta, se debe especificar el parametro que permita nulos
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                if (request.ListaInstructores != null)
                {
                    if (request.ListaInstructores.Count > 0)
                    {
                        // Obtenemos los instructores actuales en la base de datos
                        var instructoresBD = await _context.CursoInstructor.Where(x => x.CursoId == request.CursoId)
                            .ToListAsync();
                        // Eliminamos esos instructores
                        foreach (var instructor in instructoresBD)
                        {
                            _context.CursoInstructor.Remove(instructor);
                        }

                        // Insertamos los que ingresa el cliente
                        foreach (var ids in request.ListaInstructores)
                        {
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = ids
                            };

                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Unit.Value;
                throw new Exception("No se pudo modificar el curso");

            }
        }
    }
}
