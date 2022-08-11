using System.Collections.Generic;

namespace LeerData
{
    public class Instructor
    {
        public int InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }

        /// <summary>
        /// Relacion Muchos a Muchos - Muchos instructores tienen muchos cursos
        /// </summary>
        public ICollection<CursoInstructor> CursoLink { get; set; }
    }
}
