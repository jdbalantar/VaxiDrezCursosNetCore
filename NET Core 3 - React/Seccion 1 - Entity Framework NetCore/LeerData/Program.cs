using Microsoft.EntityFrameworkCore;
using System;

namespace LeerData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppVentaCursosContext())
            {
                var cursos2 = db.Curso.Include(curso => curso.PrecioPromocion).AsNoTracking();
                foreach (var curso in cursos2)
                {
                    //Console.WriteLine(curso.Titulo + "   ------------   " + curso.PrecioPromocion.PrecioActual);
                }

                var cursos1 = db.Curso.Include(c => c.ComentariosLista).AsNoTracking();
                foreach (var curso in cursos1)
                {
                    Console.WriteLine(curso.Titulo);
                    foreach (var comentario in curso.ComentariosLista)
                    {
                      //  Console.WriteLine($"Comentario: " + comentario.ComentarioTexto);
                    }
                }

                // Traigo Curso, luego a CursoInstructor, y luego Instructor
                var cursos = db.Curso.Include(x => x.InstructorLink).ThenInclude(ci => ci.Instructor);
                foreach (var curso in cursos)
                {
                    Console.WriteLine(curso.Titulo);
                    foreach (var insLink in curso.InstructorLink)
                    {
                        Console.WriteLine("Instructor: " + insLink.Instructor.Nombre);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
