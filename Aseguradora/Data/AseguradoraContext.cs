using Aseguradora.Models;
using Microsoft.EntityFrameworkCore;

namespace Aseguradora.Data
{
    public class AseguradoraContext : DbContext
    {
        public AseguradoraContext(DbContextOptions<AseguradoraContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // crea las dos claves foráneas
            modelBuilder.Entity<ClienteSeguro>().HasKey(k => new { k.CodigoSeguro, k.CedulaCliente });
            // Declara que hay una relación uno a muchos entre Cliente a ClientesSeguros
            modelBuilder.Entity<ClienteSeguro>().HasOne(e => e.Cliente).WithMany(n => n.ClientesSeguros).HasForeignKey(r => r.CedulaCliente);
            // Declara que hay una relación uno a muchos entre Seguro y ClientesSeguros
            modelBuilder.Entity<ClienteSeguro>().HasOne(e => e.Seguro).WithMany(n => n.ClientesSeguros).HasForeignKey(r => r.CodigoSeguro);
        }

        // tabla Seguros
        public DbSet<Seguro> Seguros { get; set; }
        // tabla Clientes
        public DbSet<Cliente> Clientes { get; set; }
        // tabla intermedia de Clientes y Seguros relación Muchos a Muchos
        public DbSet<ClienteSeguro> ClienteSeguro { get; set; }
    }
}
