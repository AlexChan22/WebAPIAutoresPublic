using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;


namespace WebApiAutores.DTOs
{
    
    public class AutorCreacionDTO
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio")] // 0 representa el nombre del campo
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} debe tener maximo {1} letras")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
