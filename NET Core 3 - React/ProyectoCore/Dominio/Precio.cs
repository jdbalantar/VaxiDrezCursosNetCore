using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Precio
    {
        public Guid PrecioId { get; set; }
        // Esta línea da error en SQLite
        //[Column(TypeName = "decimal(18,4")]
        public decimal PrecioActual { get; set; }
        // Esta línea da error en SQLite
        //[Column(TypeName = "decimal(18,4")]
        public decimal Promocion { get; set; }
        public Guid CursoId { get; set; }
        public Curso Curso { get; set; }

    }
}
