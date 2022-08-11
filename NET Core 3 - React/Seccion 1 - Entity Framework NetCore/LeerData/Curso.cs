using System;
using System.Collections.Generic;

namespace LeerData
{
    public class Curso
    {
        public int CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }

        /// <summary>
        /// Relacio 1 a 1 - Un Curso le pertenece a un precio
        /// Con esta propiedad de navegacion veremos hacia qué Precio está asignado el Curso
        /// </summary>
        public Precio PrecioPromocion { get; set; }

        /// <summary>
        /// Relacion 1 a Muchos - Un curso puede tener muchos comentarios
        /// Propiedad de navegacion que sirve para poder ver los comentarios asignados al curso
        /// </summary>
        public ICollection<Comentario> ComentariosLista { get; set; }

        /// <summary>
        /// Relacion Muchos a Muchos - Muchos cursos pueden tener muchos instructores
        /// </summary>
        public ICollection<CursoInstructor> InstructorLink { get; set; }
    }
}
