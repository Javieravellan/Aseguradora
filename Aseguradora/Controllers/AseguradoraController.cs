using Aseguradora.Models;
using Aseguradora.Services;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aseguradora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradoraController : ControllerBase
    {
        private readonly ServicioAseguradora servicio;

        public AseguradoraController(ServicioAseguradora servicio)
        {
            this.servicio = servicio;
        }

        [HttpGet("seguros")]
        public async Task<ActionResult<List<Seguro>>> ObtenerTodosLosSeguros()
        {
            return Ok(await servicio.ObtenerTodosLosSeguros());
        }

        [HttpGet("seguros/{codigo}")]
        public async Task<ActionResult<Seguro>> ObtenerSeguroPorCodigo(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return BadRequest("El código de seguro es requerido.");
            var seguroEncontrado = await servicio.ObtenerSeguroPorCodigo(codigo);
            if (seguroEncontrado == null) return NotFound(string.Format("No se encontró registro con código: {0}", codigo));
            return Ok(seguroEncontrado);
        }

        [HttpPost("seguros")]
        public async Task<ActionResult<Seguro>> CrearSeguro([FromBody] Seguro seguro)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ingresado = await servicio.CrearSeguro(seguro);
            return Ok(ingresado);
        }

        [HttpPut("seguros")]
        public async Task<ActionResult<Seguro>> ActualizarSeguro([FromBody] Seguro seguro)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var seguro2 = await servicio.ActualizarSeguro(seguro);
            if (seguro2 == null) return NotFound("No se pudo eliminar el seguro " + seguro!.CodigoSeguro + " porque no existe.");
            return Ok(seguro);
        }

        [HttpDelete("seguros/{codigo}")]
        public async Task<ActionResult<bool>> EliminarSeguro(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return BadRequest("El código de seguro es requerido.");
            var seguroEncontrado = await servicio.EliminarSeguro(codigo);
            if (!seguroEncontrado) return NotFound(string.Format("No se encontró registro con código: {0}", codigo));
            return Ok(seguroEncontrado); // True de Eliminado!
        }

        // End points para el crud de clientes
        [HttpGet("clientes")]
        public async Task<ActionResult<List<Cliente>>> ObtenerTodosLosClientes()
        {
            return Ok(await servicio.ObtenerTodosLosClientes());
        }

        [HttpGet("clientes/asignar")]
        public async Task<ActionResult<bool>> AsignarSeguro([FromQuery]string codigoSeguro, [FromQuery] string cedula)
        {
            return await servicio.AsignarSeguro(codigoSeguro, cedula);
        }

        [HttpGet("clientes/{cedula}")]
        public async Task<ActionResult<Cliente>> ObtenerClientePorCedula(string cedula)
        {
            if (string.IsNullOrEmpty(cedula)) return BadRequest("La cédula es requerida.");
            var seguroEncontrado = await servicio.ObtenerClientePorCedula(cedula);
            if (seguroEncontrado == null) return NotFound(string.Format("No se encontró registro con cédula: {0}", cedula));
            return Ok(seguroEncontrado);
        }

        [HttpPost("clientes")]
        public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ingresado = await servicio.CrearCliente(cliente);
            return Ok(ingresado);
        }

        [HttpPost("clientes/upload-file")]
        public async Task<ActionResult<List<Cliente>>> CrearClientesDesdeArchivo(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("No se encontró el archivo.");
            }
            var result = await servicio.CrearClientesDesdeArchivo(file);
            return Ok(result);
        }

        [HttpPut("clientes")]
        public async Task<ActionResult<Cliente>> ActualizarCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            cliente = await servicio.ActualizarCliente(cliente);
            if (cliente == null) return NotFound("No se pudo eliminar el cliente " + cliente!.CedulaCliente+ " porque no existe.");
            return Ok(cliente);
        }

        [HttpDelete("clientes/{cedula}")]
        public async Task<ActionResult<bool>> EliminarCliente(string cedula)
        {
            if (string.IsNullOrEmpty(cedula)) return BadRequest("La cédula es requerida.");
            var clienteEncontrado = await servicio.EliminarCliente(cedula);
            if (!clienteEncontrado) return NotFound(string.Format("No se encontró registro con Cédula: {0}", cedula));
            return Ok(clienteEncontrado); // True de Eliminado!
        }

        [HttpGet("clientes/buscar/{cedula}")]
        public async Task<ActionResult<List<Seguro>>> buscarSeguroPorCedula(string cedula)
        {
            try
            {
                var result = await servicio.ObtenerSegurosPorCedula(cedula);
                return result;

            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("seguros/buscar/{codigo}")]
        public async Task<ActionResult<List<Cliente>>> buscarClientesPorSeguro(string codigo)
        {
            try
            {
                var result = await servicio.ObtenerClientesPorSeguro(codigo);
                return result;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
