using MediatR;
using Persistencia;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        /// <summary>
        /// Clase que se invoca cuando se requiere hacer un registro en la BD
        /// En el IRequest no se le pone parámetro <!-- <> -->  debido a que la clase no retorna nada
        /// Esta clase solo ejecuta una accion hacia la BD
        /// </summary>
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaPublicacion { get; set; }
        }

        /// <summary>
        /// Clase que representa la lógica de la inserción
        /// </summary>
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }


            /// <summary>
            /// Clase que implementa la lógica en la inserción de informacion hacia la BD.
            /// En el IRequest no se le pone parámetro Curso <!-- <> -->  debido a que la clase no retorna nada.
            /// Esta clase solo ejecuta una accion hacia la BD, por lo que el parámetro es la clase que Ejecuta.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = new Curso
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);
                // Si el valor que devuelve el método es 0, es porque hubo un error
                // Si devuelve un valor superior a 1, es porque se guardó la información
                // Por cada transacción realizada (registro) devolverá +1
                // Es decir, si hago 5 regitros, devolverá 5
                var valor = await _context.SaveChangesAsync();

                if (valor > 0)
                    return Unit.Value;
                throw new Exception("No se guardaron los cambios en el curso");
            }
        }
    }
}
