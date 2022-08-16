using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using System.Linq;

namespace Aplicacion
{
    /// <summary>
    /// En esta clase definen los mapeos que hará AutoMapper con los DTO
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDto>()
                // Estoy incluyendo los Instructores dentro de la lista de Instructores de CursoDto
                .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructorLink.Select(a => a.Instructor).ToList()))
                .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                .ForMember(x => x.Precio, y => y.MapFrom(z => z.PrecioPromocion));

            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();
        }
    }
}
