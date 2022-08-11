namespace LeerData
{
    public class CursoInstructor
    {
        /// <summary>
        /// Llave foránea del Instructor
        /// </summary>
        public int InstructorId { get; set; }
        /// <summary>
        /// Relacion Muchos a Muchos - ANCLA
        /// </summary>
        public Instructor Instructor { get; set; }

        /// <summary>
        /// Llave foránea del Curso
        /// </summary>
        public int CursoId { get; set; }
        /// <summary>
        /// Relación Muchos a Muchos - ANCLA
        /// </summary>
        public Curso Curso { get; set; }
    }
}
