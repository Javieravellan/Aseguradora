using System.ComponentModel.DataAnnotations;

namespace Aseguradora.Models
{
    public class Seguro
    {
        [Key]
        [Required(ErrorMessage = "El campo códigoSeguro es requerido.")]
        public string CodigoSeguro { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo nombre es requerido.")]
        public string Nombre { get; set; } = string.Empty;
        [Range(1, double.MaxValue, ErrorMessage = "El valor de la suma asegurada no puede ser cero.")]
        public float SumaAsegurada { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "El valor de la prima no puede ser cero.")]
        public float PrimaSeguro { get; set; }

        public List<ClienteSeguro> ClientesSeguros { get; set; }

        public Seguro () { }
    }
}
