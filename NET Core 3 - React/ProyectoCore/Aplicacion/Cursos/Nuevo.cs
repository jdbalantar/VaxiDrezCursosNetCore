using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            public List<Guid> ListaInstructor { get; set; }
        }


        /// <summary>
        /// Clase que valida las propiedades de la clase Ejecuta - Query.
        /// FluentValidation
        /// </summary>
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
            /// En el IRequest no se le pone parámetro Detalle <!-- <> -->  debido a que la clase no retorna nada.
            /// Esta clase solo ejecuta una accion hacia la BD, por lo que el parámetro es la clase que Ejecuta.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Creamos un nuevo Id (Guid)
                Guid _cursoId = Guid.NewGuid();

                // Aquí estamos creando un nuevo curso
                var curso = new Curso
                {
                    // _cursoId es un nuevo Guid que generamos desde el código
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);

                // Insertando lista de instructores
                // Si desde Postman, enviamos la lista de instructores que queremos que estén regitrados
                // en el curso, entonces
                if (request.ListaInstructor != null)
                {
                    // Por cada ID de instructor que recibamos de Postman, haremos un registro en la tabla
                    // muchos a muchos CursoInstructor
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            // Añadimos el ID que generamos para el Detalle
                             CursoId = _cursoId,
                             // Añadimos el Id del instructor que nos llegó
                             InstructorId = id,
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

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
