using WebApiAutores.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {

        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} debe tener maximo {1} letras")]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
