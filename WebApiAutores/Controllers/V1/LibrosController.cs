using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



       

        [HttpGet("{id:int}", Name = "obtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {

            var libro = await context.Libros
                .Include(libroDB => libroDB.autoresLibros)
                .ThenInclude(autorLibro => autorLibro.Autor)
                .Include(libroBD => libroBD.comentarios).
                FirstOrDefaultAsync(x => x.Id == id);

            if(libro == null)
            {
                return NotFound();
            }

            libro.autoresLibros = libro.autoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost(Name = "crearLibro")]
        public async Task<ActionResult<Libro>> Post(LibroCreacionDTO libroDTO)
        {

            if (libroDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }


            var autoresIds = await context.Autores.Where(x => libroDTO.AutoresIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();


            if(libroDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe algun autor");            }

            /*
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"No existe el autor de Id: {libro.AutorId}");
            }
            */


            var libro = mapper.Map<Libro>(libroDTO);

            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var dto = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("obtenerLibro", new { id = libro.Id}, dto);
        }

        [HttpPut("{id:int}", Name ="actualizarLibro")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {

            var libroDB = await context.Libros
                .Include(x => x.autoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);   

            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);

            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();


        }


        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.autoresLibros != null)
            {
                for (int i = 0; i < libro.autoresLibros.Count; i++)
                {
                    libro.autoresLibros[i].Orden = i;
                }
            }
        }


        [HttpPatch("{id:int}", Name = "patchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
        
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            // Llenando los valores del libro en el LibroPatchDTO
            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            // Aplicando los cambios que vinieron del patch document
            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id:int}", Name = "borrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();


        }


    }
}
