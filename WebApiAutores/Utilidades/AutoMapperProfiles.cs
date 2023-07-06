
using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros))
                ;



            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.autoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(libroDTO => libroDTO.autores, opciones => opciones.MapFrom(MapLibroDTOAutores))
                ;

            CreateMap<LibroPatchDTO, Libro>().ReverseMap();


            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
            
        }

        private List<AutoresLibros> MapAutoresLibros(LibroCreacionDTO dto, Libro libro)
        {

        
            var res = new List<AutoresLibros>();

            if (dto.AutoresIds == null)
            {
                return res;
            }

            foreach (var autorId in dto.AutoresIds)
            {
                res.Add(new AutoresLibros()
                {
                    AutorId = autorId
                });
            }

            return res;
        }


        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO dto )
        {
            var resultado = new List<AutorDTO>();

            if (libro.autoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in libro.autoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                }) ;
            }

            return resultado;

        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO dto)
        {
            var resultado = new List<LibroDTO>();

            if (autor.autoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in autor.autoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }

            return resultado;

        }

    }
}
