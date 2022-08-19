using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
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
                // Eliminamos primero todas las referencias que tenga el curso
                // Comentarios, Precios, Instructores, etc

                var instructorDB = _context.CursoInstructor.Where(x => x.CursoId == request.Id);
                foreach (var instructor in instructorDB)
                {
                    _context.CursoInstructor.Remove(instructor);
                }

                /* Lógica para eliminar los comentarios asignados al curso que se va a eliminar */
                var comentariosDB = _context.Comentario.Where(x => x.CursoId == request.Id);
                foreach (var comentario in comentariosDB)
                {
                    _context.Comentario.Remove(comentario);
                }

                /* Lógica para eliminar el precio al curso que se va a eliminar */
                var precioDB = await _context.Precio.Where(x => x.CursoId == request.Id).FirstOrDefaultAsync();
                if (precioDB != null)
                    _context.Precio.Remove(precioDB);

                var curso = await _context.Curso.FindAsync(request.Id);

                if (curso == null)
                    // Haciendo uso de la Excepción ManejadorExcepcion, para cuando haya un error en el API
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontró el curso" });

                _context.Remove(curso);
                var value = await _context.SaveChangesAsync();
                if (value > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo modificar el curso");
            }
        }
    }
}
