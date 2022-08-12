using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public int CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
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
                    throw new Exception("Curso no encontrado");

                // Si no le estoy enviando nada, no actualice, déjelo igual
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                // Los DateTime no permiten NULL por defecto
                // Por eso, en la clase Ejecuta, se debe especificar el parametro que permita nulos
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Unit.Value;
                throw new Exception("No se pudo modificar el curso");

            }
        }
    }
}
