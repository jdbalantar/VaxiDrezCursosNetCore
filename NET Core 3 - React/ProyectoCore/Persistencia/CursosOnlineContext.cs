using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : DbContext
    {
        public CursosOnlineContext(DbContextOptions<CursosOnlineContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Se deja esta linea para especificar que habrán dos PK en la tabla
            modelBuilder.Entity<CursoInstructor>().HasKey(x => new { x.InstructorId, x.CursoId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public CursoInstructor CursoInstructor { get; set; }
        public DbSet<Precio> Precio { get; set; }
    }
}