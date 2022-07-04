using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Aseguradora.Models
{
    [Keyless]
    public class ClienteSeguro
    {
        public string CodigoSeguro { get; set; }
        public string CedulaCliente { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; }
        [JsonIgnore]
        public Seguro Seguro { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
