using System.ComponentModel.DataAnnotations;

namespace Aseguradora.Models
{
    public class Cliente
    {
        [Key]
        //[Required(ErrorMessage = "El campo Cédula es requerido.")]
        public string CedulaCliente { get; set; } = string.Empty;
        //[Required(ErrorMessage = "El nombre del cliente es requerido")]
        public string NombreCliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        //[Range(0, 100, ErrorMessage = "La edad debe ser un valor válido.")]
        public short Edad { get; set; }

        public List<ClienteSeguro> ClientesSeguros { get; set; }

        public Cliente() { }
    }
}
