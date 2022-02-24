using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiContribuyente.Entidades
{
    public class Contribuyente
    {
        public int Id { get; set; }
            
        [Required(ErrorMessage = "El {0} debe de ser ingresado obligatoriamente")]
        [StringLength(15, ErrorMessage = "El {0} debe de tener 15 caracteres como máximo")]
        public string Nombre { get; set; }

        [NotMapped]
        public int Edad  { get; set; }

        [NotMapped]
        [CreditCard]
        public string Tarjeta { get; set; }

        [NotMapped]
        [Url]
        public string Url { get; set; }

        public List<Declaracion> Declaraciones { get; set;}
    }
}
