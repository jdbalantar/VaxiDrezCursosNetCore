namespace LeerData
{
    public class Precio
    {
        public int PrecioId { get; set; }
        /// <summary>
        /// El tipo de dato Money en C# es "decimal"
        /// </summary>
        public decimal PrecioActual { get; set; }
        public decimal Promocion { get; set; }
        
        /// <summary>
        /// Relación 1 a 1 - Un precio tendrá asignado un curso
        /// </summary>
        public int CursoId { get; set; }

        /// <summary>
        /// Es el ancla para que desde la clase Curso se pueda acceder al precio
        /// </summary>
        public Curso Curso { get; set; }
    }
}
