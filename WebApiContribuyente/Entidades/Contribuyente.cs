using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiContribuyente.Validaciones;

namespace WebApiContribuyente.Entidades
{
    public class Contribuyente : IValidatableObject
    {
        public int Id { get; set; }
            
        [Required(ErrorMessage = "El {0} debe de ser ingresado obligatoriamente")]
        [StringLength(15, ErrorMessage = "El {0} debe de tener 15 caracteres como máximo")]
        //[PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        [Range(18,100, ErrorMessage = "El campo Edad no se encuentra dentro del rango")]
        [NotMapped]
        public int Edad  { get; set; }

        [NotMapped]
        [CreditCard]
        public string Tarjeta { get; set; }

        [NotMapped]
        [Url]
        public string Url { get; set; }

        public List<Declaracion> Declaraciones { get; set;}

        [NotMapped]
        public int Menor { get; set; }

        [NotMapped]
        public int Mayor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            /* Para que se ejecuten deben de cumplirse primero con las reglas por Atributo, ejemplo: Range
             * Tomar a consideración que primero se ejecutarán las validaciones mapeadas en los atributos
             * y posteriormente las delcaraciones en la entidad
             */
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula",
                        new String[] { nameof(Nombre) });
                }
            }

            if (Menor > Mayor)
            {
                yield return new ValidationResult("Este valor no puede ser más grande que el campo Mayor",
                    new String[] { nameof(Menor) });
            }
        }
    }
}
