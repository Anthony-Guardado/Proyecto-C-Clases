using InventaMeCF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace InventaMeCF.Controllers
{
    public class PruebaController : Controller
    {
        private readonly InventaMeCFContext _context;


        public PruebaController(InventaMeCFContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int[] a = new int[] { 10, 20, 30, 40 };
            ViewBag.Comentario1 = "Comentario 1";
            ViewBag.a1 = a;
            ViewBag.Meses = new string[]
{
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
          
        };

            /* using SHA256 mySHA256 = SHA256.Create();
             byte[] datos = new byte[] { 65, 83, 77, 71 };
             byte[] hash = mySHA256.ComputeHash(datos);

             ViewBag.hashValue = hash;*/

            SHA256 mySHA256 = SHA256.Create();

            // byte[] datos = new byte[] { 109, 97, 99, 118 };
            byte[] datos = Encoding.UTF8.GetBytes("ASMG");
            byte[] hashValue = mySHA256.ComputeHash(datos);
            ViewBag.hashValue = hashValue;

            string hashValueHexadecimal = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
            ViewBag.hashValueHexadecimal = hashValueHexadecimal;

            //return View(await _context.Productos.ToListAsync());
            var productos = await _context.Productos
             .Include(p => p.Marca)
             .ToListAsync();

            return View(productos);
        }

        public async Task<IActionResult> Crear()
        {
            ViewBag.Marcas = await _context.Marcas
                .OrderBy(m => m.Name)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                })
                .ToListAsync();

            return View(new Producto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("Id,Nombre,Descripcion,PrecioUnitario,MarcaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Tarjeta(int? id)
        {
            var prod1 = await _context.Productos.FindAsync(id);
            if (prod1 == null)
            {
                return View(null);
            }
            return View(prod1);

        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListaPrecioMedio(bool mayores)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }
            var precio_medio = await _context.Productos.AverageAsync(a => a.PrecioUnitario);
            var productos = (mayores) ? await _context.Productos.Where(a => a.PrecioUnitario > precio_medio).ToListAsync() : await _context.Productos.Where(a => a.PrecioUnitario < precio_medio).ToListAsync();
            ViewBag.Promedio = precio_medio;
            ViewBag.Titulo = (mayores) ? "mayores" : "menores";
            return View(productos);
        }


    }
}
