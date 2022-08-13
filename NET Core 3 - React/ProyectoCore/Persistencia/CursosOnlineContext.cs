using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions<CursosOnlineContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            // Se deja esta linea para especificar que habrán dos PK en la tabla
            modelBuilder.Entity<CursoInstructor>().HasKey(x => new { x.InstructorId, x.CursoId });

            // Sentencia de código para crear el archivo de migración
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Curso> Curso { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public CursoInstructor CursoInstructor { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public Usuario Usuario { get; set; }
    }
}