# grafico_estadistico_sql_server

<img width="2549" height="1078" alt="image" src="https://github.com/user-attachments/assets/d86a2b48-6700-4705-9fe7-5598014d3fb4" />

Perfecto, te ayudaré paso a paso a crear una aplicación ASP.NET MVC con conexión a SQL Server, que muestre un gráfico estadístico tipo Excel, 
donde cada conjunto de datos (representado por un color) proviene de una tabla diferente de la base de datos.

✅ Requisitos

Visual Studio 2022 o superior

SQL Server (puede ser LocalDB)

Paquete NuGet: Chart.js, Entity Framework, Microsoft.Data.SqlClient

🗃️ Paso 1: Crear la Base de Datos

Vamos a crear una base de datos con 5 tablas. Puedes usar este script en SQL Server:

CREATE DATABASE EstadisticasDB;
GO
USE EstadisticasDB;
GO

-- Tabla 1: Ventas
CREATE TABLE Ventas (
    Id INT PRIMARY KEY IDENTITY,
    Monto DECIMAL(10,2),
    Fecha DATE
);

-- Tabla 2: Compras
CREATE TABLE Compras (
    Id INT PRIMARY KEY IDENTITY,
    Monto DECIMAL(10,2),
    Fecha DATE
);

-- Tabla 3: Empleados
CREATE TABLE Empleados (
    Id INT PRIMARY KEY IDENTITY,
    Edad INT,
    FechaIngreso DATE
);

-- Tabla 4: Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY,
    Edad INT,
    FechaRegistro DATE
);

-- Tabla 5: Productos
CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY,
    Precio DECIMAL(10,2),
    FechaAlta DATE
);


Llena las tablas con algunos datos de ejemplo.

🛠️ Paso 2: Crear el Proyecto en Visual Studio

Abre Visual Studio → Crear nuevo proyecto

Elige: ASP.NET Web Application (.NET Framework)

Nómbralo MvcGraficosApp

Selecciona MVC y desactiva la autenticación

🔌 Paso 3: Conexión a la Base de Datos
1. Instala Entity Framework

En el Administrador de paquetes NuGet, instala:

Install-Package EntityFramework

2. Agrega la cadena de conexión en Web.config
<connectionStrings>
  <add name="EstadisticasDB" connectionString="Data Source=.;Initial Catalog=EstadisticasDB;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>

3. Crea el Modelo y DbContext

En la carpeta Models, agrega 5 clases:

public class Venta
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}

public class Compra
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}

public class Empleado
{
    public int Id { get; set; }
    public int Edad { get; set; }
    public DateTime FechaIngreso { get; set; }
}

public class Cliente
{
    public int Id { get; set; }
    public int Edad { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class Producto
{
    public int Id { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaAlta { get; set; }
}


Ahora crea el DbContext:

public class EstadisticasDbContext : DbContext
{
    public EstadisticasDbContext() : base("EstadisticasDB") {}

    public DbSet<Venta> Ventas { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Producto> Productos { get; set; }
}

📊 Paso 4: Crear el Controlador y Vista

Crea un controlador: GraficosController

public class GraficosController : Controller
{
    private EstadisticasDbContext db = new EstadisticasDbContext();

    public ActionResult Index()
    {
        // Resumen por tabla (sumatoria)
        var ventas = db.Ventas.Sum(v => v.Monto);
        var compras = db.Compras.Sum(c => c.Monto);
        var empleados = db.Empleados.Count();
        var clientes = db.Clientes.Count();
        var productos = db.Productos.Count();

        ViewBag.Ventas = ventas;
        ViewBag.Compras = compras;
        ViewBag.Empleados = empleados;
        ViewBag.Clientes = clientes;
        ViewBag.Productos = productos;

        return View();
    }
}


Crea la vista: Views/Graficos/Index.cshtml

@{
    ViewBag.Title = "Gráfico Estadístico";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Gráfico Estadístico</h2>

<canvas id="myChart" width="400" height="200"></canvas>

<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar', // tipo de gráfico
        data: {
            labels: ['Ventas', 'Compras', 'Empleados', 'Clientes', 'Productos'],
            datasets: [{
                label: 'Totales por Categoría',
                data: [
                    @ViewBag.Ventas,
                    @ViewBag.Compras,
                    @ViewBag.Empleados,
                    @ViewBag.Clientes,
                    @ViewBag.Productos
                ],
                backgroundColor: [
                    'rgba(75, 192, 192, 0.6)',  // Ventas - verde agua
                    'rgba(255, 99, 132, 0.6)',  // Compras - rojo
                    'rgba(54, 162, 235, 0.6)',  // Empleados - azul
                    'rgba(255, 206, 86, 0.6)',  // Clientes - amarillo
                    'rgba(153, 102, 255, 0.6)'  // Productos - morado
                ],
                borderColor: [
                    'rgba(75, 192, 192, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(153, 102, 255, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
</script>

🔄 Paso 5: Configura la Ruta

En RouteConfig.cs:

routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Graficos", action = "Index", id = UrlParameter.Optional }
);

✅ Resultado Final

Se mostrará un gráfico de barras con los datos estadísticos de cada tabla.

Cada barra (color) representa una tabla distinta.

Puedes cambiar el tipo de gráfico a line, pie, etc., si lo deseas.

---------------------------//------------------------//-------------------//------------------//---------------------//--------------------------//------------------------//

# 🔄 ASP.NET Core MVC con Gráfico Estadístico y SQL Server

Este documento adapta el proceso para crear una aplicación **ASP.NET Core MVC** que usa **Entity Framework Core**, **SQL Server** y **Chart.js** para mostrar un gráfico estadístico.

---

## 🧭 Estructura General del Proyecto

- ASP.NET Core MVC (.NET 6, 7 o 8)
- Conexión a SQL Server
- Entity Framework Core (EF Core)
- Front-end con Chart.js

---

## 🧱 Paso 1: Crear el Proyecto

1. Abre **Visual Studio**
2. Selecciona **ASP.NET Core Web App (Model-View-Controller)**
3. Nómbralo, por ejemplo: `MvcGraficosApp`
4. Elige .NET 6, 7 o 8 y asegúrate de seleccionar la opción **MVC**
5. Clic en **Crear**

---

## 📦 Paso 2: Instalar Paquetes NuGet

Ejecuta en la consola de NuGet o terminal:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
🗃️ Paso 3: Configurar la Cadena de Conexión
En appsettings.json, agrega:

json
Copiar código
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=EstadisticasDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
📂 Paso 4: Crear Modelos y DbContext
🧱 Modelos (Models/)
csharp
Copiar código
public class Venta
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}

public class Compra
{
    public int Id { get; set; }
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; }
}

public class Empleado
{
    public int Id { get; set; }
    public int Edad { get; set; }
    public DateTime FechaIngreso { get; set; }
}

public class Cliente
{
    public int Id { get; set; }
    public int Edad { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class Producto
{
    public int Id { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaAlta { get; set; }
}
📘 DbContext
csharp
Copiar código
using Microsoft.EntityFrameworkCore;

public class EstadisticasDbContext : DbContext
{
    public EstadisticasDbContext(DbContextOptions<EstadisticasDbContext> options) : base(options) { }

    public DbSet<Venta> Ventas { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Producto> Productos { get; set; }
}
🧷 Paso 5: Configurar EF Core en Program.cs
Agrega esta línea dentro de builder.Services:

csharp
Copiar código
builder.Services.AddDbContext<EstadisticasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
📈 Paso 6: Crear Controlador y Vista
🧩 Controlador: GraficosController.cs
csharp
Copiar código
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class GraficosController : Controller
{
    private readonly EstadisticasDbContext _context;

    public GraficosController(EstadisticasDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var ventas = _context.Ventas.Sum(v => v.Monto);
        var compras = _context.Compras.Sum(c => c.Monto);
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
🎨 Vista: Views/Graficos/Index.cshtml
html
Copiar código
@{
    ViewData["Title"] = "Gráfico Estadístico";
}

<h2>Gráfico Estadístico</h2>

<canvas id="myChart" width="400" height="200"></canvas>

<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
    const ctx = document.getElementById('myChart').getContext('2d');
    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Ventas', 'Compras', 'Empleados', 'Clientes', 'Productos'],
            datasets: [{
                label: 'Estadísticas por Tabla',
                data: [
                    @ViewBag.Ventas,
                    @ViewBag.Compras,
                    @ViewBag.Empleados,
                    @ViewBag.Clientes,
                    @ViewBag.Productos
                ],
                backgroundColor: [
                    'rgba(75, 192, 192, 0.6)',
                    'rgba(255, 99, 132, 0.6)',
                    'rgba(54, 162, 235, 0.6)',
                    'rgba(255, 206, 86, 0.6)',
                    'rgba(153, 102, 255, 0.6)'
                ],
                borderColor: [
                    'rgba(75, 192, 192, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(153, 102, 255, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
</script>
🛠️ Paso 7: Crear y Aplicar Migraciones
En la consola del Administrador de Paquetes NuGet:

bash
Copiar código
Add-Migration InitialCreate
Update-Database
Esto creará la base de datos EstadisticasDB con las tablas correspondientes.

🚀 Paso 8: Ejecutar la App
Puedes configurar la vista de GraficosController como inicio en Program.cs (opcional).

Ejecuta la aplicación desde Visual Studio.

Verás un gráfico de barras que representa los datos de las 5 tablas, cada una con un color distinto.
