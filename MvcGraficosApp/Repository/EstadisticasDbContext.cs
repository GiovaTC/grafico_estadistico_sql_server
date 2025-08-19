using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using MvcGraficosApp.Models;

namespace MvcGraficosApp.Repository
{
    public class EstadisticasDbContext : DbContext
    {
        public EstadisticasDbContext(DbContextOptions<EstadisticasDbContext> options) : base(options) { }

        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
