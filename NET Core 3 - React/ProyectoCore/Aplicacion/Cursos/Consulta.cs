using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

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
        /// Se devolverá una Lista de objetos (Detalle) de tipo IRequest
        /// </summary>
        public class ListaCursos : IRequest<List<CursoDto>>
        {

        }

        /// <summary>
        /// Esta clase representa la lógica que se usará cuando se ejecute la clase Consulta
        /// </summary>
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            /// <summary>
            /// En este método se configura la lógica de la consulta. Aquí configuramos la forma en la
            /// que se devolverá la información
            /// </summary>
            /// <param name="request">Representa lo que hay en la Lista de Cursos</param>
            /// <param name="cancellationToken">En caso de que el usuario cancele el Request, se manejará con este atributo</param>
            /// <returns></returns>
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructorLink)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();

                // El método map pide dos parametros
                // Primer parámetro es el tipo de dato origen
                // El segundo parámetro es el tipo de dato en el que deseo que se convierta
                // cursos viene siendo la fuente de origen
                var cursosDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);
                return cursosDto;
            }
        }
    }
}
