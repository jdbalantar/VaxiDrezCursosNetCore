using System;
using System.Collections.Generic;
using System.Text;

namespace LeerData
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }

        /// <summary>
        /// Relacion de Muchos a 1
        /// Muchos comentarios pueden permanecer a un curso
        /// Llave foranea que contiene el Id del curso al que se le insertará el comentario
        /// </summary>
        public int CursoId { get; set; }

        /// <summary>
        /// Ancla a nivel de objetos para que desde la clase Curso se pueda acceder a los comentarios
        /// </summary>
        public Curso Curso { get; set; }
    }
}
