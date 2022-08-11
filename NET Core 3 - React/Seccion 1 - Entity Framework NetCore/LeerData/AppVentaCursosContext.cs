using Microsoft.EntityFrameworkCore;

namespace LeerData
{
    public class AppVentaCursosContext : DbContext
    {


        private const string connectionString = @"Data Source=BALANTA\SQLSERVER; Initial Catalog=CursosOnline; Integrated Security=True";

        /// <summary>
        /// Método para definir la conexión al servidor que me interesa
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Con esto le digo a Entity Framework que la clase Instructor tendrá dos llaves primarias
            modelBuilder.Entity<CursoInstructor>().HasKey(x => new { x.InstructorId, x.CursoId });
        }

        public DbSet<Curso> Curso { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
    }
}
