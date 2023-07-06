using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor
    {

        // Primero de ejecutan las validaciones por attributo
        // Cuando esas validaciones pasan, se chequean las validaciones de modelo

        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")] // 0 representa el nombre del campo
        [StringLength(maximumLength:120, ErrorMessage = "El campo {0} debe tener maximo {1} letras")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public List<AutoresLibros> autoresLibros { get; set; }

        /* [Range(18, 120)]
         [NotMapped]
         public int Edad { get; set; }

         // Validacion del formato de una tarjeta (no necesariamente que sea real)
         [CreditCard]
         [NotMapped]
         public string TarjetaDeCredito { get; set; }

         [Url]
         [NotMapped]
         public int URL { get; set; }

         */


        //public List<Libro> Libros { get; set; }



        /*
        // yield -> retorna un elemento (en este caso una validacion) a las vez
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscuala", 
                        new string[] {nameof(Nombre)});
                }
            }
        }*/
    }
}
