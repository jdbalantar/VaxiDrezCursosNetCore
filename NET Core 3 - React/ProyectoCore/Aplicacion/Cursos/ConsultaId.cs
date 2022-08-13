using System.Net;
using Dominio;
using MediatR;
using Persistencia;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;

namespace Aplicacion.Cursos
{
    public class ConsultaId : IRequest<Curso>
    {
        public class CursoUnico : IRequest<Curso>
        {
            public int Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, Curso>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }


            public async Task<Curso> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                    // Haciendo uso de la Excepción ManejadorExcepcion, para cuando haya un error en el API
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontró el curso" });
                return curso;
            }
        }
    }
}
