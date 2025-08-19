using Microsoft.AspNetCore.Mvc;
using MvcGraficosApp.Repository;
using System.Linq;

namespace MvcGraficosApp.Controllers
{
    public class GraficosController : Controller
    {
        private readonly EstadisticasDbContext _context;

        public GraficosController(EstadisticasDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ventas = _context.Ventas.Sum(v => (decimal?)v.Monto) ?? 0;
            var compras = _context.Compras.Sum(c => (decimal?)c.Monto) ?? 0;
            var empleados = _context.Empleados.Count();
            var clientes = _context.Clientes.Count();
            var productos = _context.Productos.Count();

            ViewBag.Ventas = ventas;
            ViewBag.Compras = compras;
            ViewBag.Empleados = empleados;
            ViewBag.Clientes = clientes;
            ViewBag.Productos = productos;

            return View();
        }
    }
}
