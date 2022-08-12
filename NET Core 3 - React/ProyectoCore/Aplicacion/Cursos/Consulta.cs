using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    // - MediatR - MediatR - MediatR - MediatR
    /// <summary>
    /// Clase que representa la Consulta de Cursos que se hace hacia la base de datos
    /// </summary>
    public class Consulta
    {
        /// <summary>
        /// Esta clase representa lo que se va a devolver cuando se ejecute la clase Consulta
        /// Se devolverá una Lista de objetos (Curso) de tipo IRequest
        /// </summary>
        public class ListaCursos : IRequest<List<Curso>>
        {

        }

        /// <summary>
        /// Esta clase representa la lógica que se usará cuando se ejecute la clase Consulta
        /// </summary>
        public class Manejador : IRequestHandler<ListaCursos, List<Curso>>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            /// <summary>
            /// En este método se configura la lógica de la consulta. Aquí configuramos la forma en la
            /// que se devolverá la información
            /// </summary>
            /// <param name="request">Representa lo que hay en la Lista de Cursos</param>
            /// <param name="cancellationToken">En caso de que el usuario cancele el Request, se manejará con este atributo</param>
            /// <returns></returns>
            public async Task<List<Curso>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Curso.ToListAsync();
                return cursos;
            }
        }
    }
}
