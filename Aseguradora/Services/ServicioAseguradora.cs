using Aseguradora.Models;
//using Microsoft.AspNetCore.Http;

namespace Aseguradora.Services
{
    public class ServicioAseguradora
    {
        private readonly AseguradoraContext context;

        public ServicioAseguradora(AseguradoraContext context)
        {
            this.context = context;
        }

        public async Task<List<Seguro>> ObtenerTodosLosSeguros()
        {
            return await context.Seguros.ToListAsync();
        }

        public async Task<Seguro?> ObtenerSeguroPorCodigo(string CodigoSeguro)
        {
            var seguroEncontrado = await context.Seguros.FindAsync(CodigoSeguro);
            return seguroEncontrado;
        }

        public async Task<List<Seguro>> ObtenerSegurosPorCedula(string cedula)
        {
            if (cedula == "" || (cedula.Length < 5 && cedula.Length > 10)) {
                throw new Exception("Cédula no debe estar vacía ni tener menos de 5 caracteres");
            }
            var lista = await context.ClienteSeguro
                .Include(x => x.Seguro)
                .Where(x => x.CedulaCliente == cedula)
                .ToListAsync();

            return lista.Select(x => x.Seguro).ToList();
        }
        public async Task<List<Cliente>> ObtenerClientesPorSeguro(string codigo)
        {
            if (codigo == "" || (codigo.Length < 5 && codigo.Length > 10))
            {
                throw new Exception("Código no debe estar vacío ni tener menos de 5 caracteres");
            }
            var lista = await context.ClienteSeguro
                .Include(x => x.Cliente)
                .Where(x => x.CodigoSeguro == codigo)
                .ToListAsync();

            return lista.Select(x => x.Cliente).ToList();
        }

        public async Task<Seguro> CrearSeguro(Seguro seguro)
        {
            context.Seguros.Add(seguro);
            await context.SaveChangesAsync();
            return seguro;
        }

        public async Task<Seguro> ActualizarSeguro(Seguro seguro)
        {
            var encontrado = await context.Seguros.FindAsync(seguro.CodigoSeguro);
            if (encontrado == null) return null;

            encontrado.Nombre = seguro.Nombre;
            encontrado.SumaAsegurada = seguro.SumaAsegurada;
            encontrado.PrimaSeguro = seguro.PrimaSeguro;
            await context.SaveChangesAsync();
            return encontrado;
        } 

        public async Task<bool> EliminarSeguro(string CodigoSeguro)
        {
            var seguroEncontrado = await context.Seguros.FindAsync(CodigoSeguro);
            if (seguroEncontrado == null) return false;
            context.Seguros.Remove(seguroEncontrado);
            await context.SaveChangesAsync();
            return true;
        }

        // Crud para los asegurados
        public async Task<List<Cliente>> ObtenerTodosLosClientes()
        {
            var clientes = await context.Clientes.Include(x => x.ClientesSeguros)
                .ToListAsync();
            return clientes;
        }

        public async Task<Cliente?> ObtenerClientePorCedula(string Cedula)
        {
            var clienteEncontrado = await context.Clientes.Include(x => x.ClientesSeguros)
                .Where(x => x.CedulaCliente == Cedula)
                .FirstAsync(); //FindAsync(Cedula);
            return clienteEncontrado;
        }

        public async Task<Cliente> CrearCliente(Cliente cliente)
        {
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();
            return cliente;
        }

        public async Task<List<Cliente>> CrearClientesDesdeArchivo(IFormFile file)
        {
            var listaClientesAgregados = new List<Cliente>();

            try
            {
                if (file.Length > 0)
                {
                    // leer el archivo linea a linea y cada linea separarla por coma.
                    var reader = new StreamReader(file.OpenReadStream());
                    using (reader)
                    {
                        while(reader.Peek() >= 0)
                        {
                            var currentLine = await reader.ReadLineAsync();
                            // separar el contenido de la linea por coma
                            var datosLinea = currentLine!.Split(",");
                            var nuevoCliente = new Cliente
                            {
                                CedulaCliente = datosLinea[0], // cédula
                                NombreCliente = datosLinea[1],
                                Telefono = datosLinea[2],
                                Edad = (short)Convert.ToSingle(datosLinea[3]),
                                ClientesSeguros = { }
                            };
                            var encontrado = await ObtenerClientePorCedula(nuevoCliente.CedulaCliente);
                            if (encontrado == null) listaClientesAgregados.Add(nuevoCliente);
                        }
                        context.Clientes.AddRange(listaClientesAgregados); // agrega todos los clientes de golpe.
                        await context.SaveChangesAsync();
                    }
                    return listaClientesAgregados;
                }
                else
                {
                    return await Task.Run(() => listaClientesAgregados); // lista vacía.
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar leer el archivo.", ex);
            }
        }

        public async Task<Cliente> ActualizarCliente(Cliente cliente)
        {
            var encontrado = await context.Clientes.FindAsync(cliente.CedulaCliente);
            if (encontrado == null) return null;

            encontrado.NombreCliente = cliente.NombreCliente;
            encontrado.Telefono = cliente.Telefono;
            encontrado.Edad = cliente.Edad;

            await context.SaveChangesAsync();
            return encontrado;
        }

        public async Task<bool> EliminarCliente(string Cedula)
        {
            var clienteEncontrado = await context.Clientes.FindAsync(Cedula);
            if (clienteEncontrado == null) return false;
            context.Clientes.Remove(clienteEncontrado);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AsignarSeguro(string codigo, string cedula)
        {
            // buscar cliente
            var cliente = await context.Clientes.FindAsync(cedula);
            var seguro = await context.Seguros.FindAsync(codigo);

            if (cliente != null && seguro != null)
            {
                var asegurado = new ClienteSeguro()
                {
                    CedulaCliente = cliente.CedulaCliente,
                    CodigoSeguro = seguro.CodigoSeguro,
                    FechaAlta = DateTime.Now,
                    Cliente = cliente,
                    Seguro = seguro
                };
                await context.ClienteSeguro.AddAsync(asegurado);
                await context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
