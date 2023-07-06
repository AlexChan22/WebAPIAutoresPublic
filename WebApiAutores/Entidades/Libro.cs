using WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.Entidades
{
    public class Libro
    {

        public int Id { get; set; }

        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} debe tener maximo {1} letras")]
        public string Titulo { get; set; }

        public DateTime? FechaPublicacion  { get; set; }
        public List<Comentario> comentarios { get; set; }

        public List<AutoresLibros> autoresLibros { get; set; }  



    }
}
