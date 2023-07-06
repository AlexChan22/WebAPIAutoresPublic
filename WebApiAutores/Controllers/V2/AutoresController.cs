using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApiAutores.Utilidades;
namespace WebApiAutores.Controllers.V2
{
    // Piensa en tagging en Java @Controller -> [ApiController]
    [ApiController]
   // [Route("api/v2/autores")]
    [Route("api/autores")]
    [CabeceraEstaPresente("x-version", "2")]
    // [Route("api/[controller]")] -> toma el nombre del controlador
    // [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;
        private readonly IConfiguration configuration;

        // private readonly ILogger<AutoresController> logger;


        // Logs que corresponden a una clase T (en este caso AutoresController)
        public AutoresController(ApplicationDbContext context, 
            IMapper mapper, 
            IAuthorizationService authorizationService
            )
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }





        // [HttpGet("listado")] -> puede tener varias rutas que hacen lo mismo
        // [HttpGet("/listado")] -> reemplaza la ruta definida del controlador (define un nuevo endpoint)
        // Se puede retornar una Lista en vez de un action result (hay casos especificos para retornar un action result)
        // como cuando quieres retornar una action como un Ok() or NotFound()
        // Action result te dice que retorna on un action o el valor que le asignes dentro de los <>
        // IActionResult retornas una accion que contiene los recursos en el body
        // Es como ActionResult pero menos especifico (no es generica como ActionResult)

        // Programacion asynchrona 
        // Se una normalmente para I/O operations, consultas a una API, leer archivos, consultar una DB
        // Task : un promise que se va a retornar en el futuro, ayuda a que se ejecuten otros acciones
        // mientras esperas
        // Task<T>: el Task promete retornar el valor T cuando este disponible
        // Query string en una URL va despues de un ?, EX: ?nombre=Pepa&apellido=Bravo
        // public async Task<ActionResult<List<Autor>>> Get([FromHeader] int valor, [FromQuery] string nombre)
        [HttpGet(Name = "obtenerAutoresv2")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {

            // tipos de log: critical, error, warning, information, debug y trace
            // logger.LogInformation("Obteniendo autores");
            var autores = await context.Autores.ToListAsync();

            autores.ForEach(autor => autor.Nombre = autor.Nombre.ToUpper());
            return mapper.Map<List<AutorDTO>>(autores);
          
        }

        [HttpGet("{id:int}", Name="obtenerAutorv2")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {

            var autor = await context.Autores
                .Include(autorDB => autorDB.autoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorDB => autorDB.Id == id);

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            if (autor == null)
            {
                return NotFound();
            }
            // tipos de log: critical, error, warning, information, debug y trace
            // logger.LogInformation("Obteniendo autores");
            var dto = mapper.Map<AutorDTOConLibros>(autor);
            
            return dto;
        }

        


        [HttpGet("{nombre}", Name = "obtenerAutorPorNombrev2")]
        public async Task<ActionResult<List<AutorDTO>>> GetPorNombre(string nombre)
        {

            var autores = await context.Autores.Where(autorDB => autorDB.Nombre.Contains(nombre)).ToListAsync();

            if (autores == null)
            {
                return NotFound();
            }
            // tipos de log: critical, error, warning, information, debug y trace
            // logger.LogInformation("Obteniendo autores");
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost(Name = "crearAutorv2")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {

            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (existeAutor)
            {
                return BadRequest($"Ya existe el Autor {autorCreacionDTO.Nombre}");
            }

            // Auto mapping
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            // Add no lo inserta -> pero lo deja en el context como preparado para que se inserte luego de guardar
            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDto = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("obtenerAutorv2", new { id = autor.Id}, autorDto);
        }

       [HttpPut("{id:int}", Name = "actualizarAutorv2")]
        // [HttpPut("{id:int}/{param2?}")] -> agregar otros parametros con / y hacerlo opcional con ?
        // Y para pasarle un valor default se le pone un = y el valor por defecto
        public async Task<ActionResult> Put (AutorCreacionDTO autorCreacionDTO, [FromRoute] int id)
       {
            

            var existeAutor = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existeAutor)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
       }


        [HttpDelete("{id:int}", Name = "borrarAutorv2")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if(!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();


        }



    }
}
