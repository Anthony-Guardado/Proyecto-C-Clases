using InventaMeCF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventaMeCF.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class ProductoController : Controller
    {
        private readonly InventaMeCFContext _context;

        public ProductoController(InventaMeCFContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? productoMarca, string cadenaBusqueda)
        {
            if (_context.Productos == null || _context.Marcas == null)
            {
                return Problem("El conjunto de datos está vacío.");
            }

            // Usamos Name
            IQueryable<Marca> marcaQuery = from m in _context.Marcas
                                           orderby m.Name
                                           select m;

            var productos = from p in _context.Productos
                            select p;

            // Filtro por nombre de producto
            if (!string.IsNullOrEmpty(cadenaBusqueda))
            {
                productos = productos.Where(s => s.Nombre!.ToUpper().Contains(cadenaBusqueda.ToUpper()));
            }

            // Filtro por marca
            if (productoMarca.HasValue && productoMarca.Value != 0)
            {
                productos = productos.Where(x => x.MarcaId == productoMarca.Value);
            }

            var productoMarcaVM = new ProductoMarcaViewModel
            {
                // CAMBIO CLAVE: Usar "Name" para el texto visible en el dropdown
                Marcas = new SelectList(await marcaQuery.ToListAsync(), "Id", "Name"),
                Productos = await productos.ToListAsync()
            };

            return View(productoMarcaVM);
        }
    }
}