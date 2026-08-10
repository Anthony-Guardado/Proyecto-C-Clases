using ClosedXML.Excel;
using InventaMeCF.Models;
using InventaMeCF.Pdf;
using InventaMeCF.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


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

        /*public async Task<IActionResult> Index(int? productoMarca, string cadenaBusqueda)
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
        }*/

        public async Task<IActionResult> Index(int pg = 1, int? productoMarca = null, string? cadenaBusqueda = null)
        {
            if (_context.Productos == null)
            {
                return Problem("El conjunto 'InventaMeCFContext.Productos' está vacío.");
            }

            var productos = _context.Productos.Include(p => p.Marca).AsQueryable();

            if (!string.IsNullOrEmpty(cadenaBusqueda))
            {
                productos = productos.Where(s => s.Nombre!.ToUpper().Contains(cadenaBusqueda.ToUpper()));
            }

            if (productoMarca.HasValue && productoMarca.Value != 0)
            {
                productos = productos.Where(x => x.MarcaId == productoMarca.Value);
            }

            var lista = await productos.ToListAsync();

            var paginacion = new PaginacionV3(lista.Count, pg, 20, "Producto");

            // Inicialización segura del diccionario de parámetros
            paginacion.Parametros ??= new Dictionary<string, string>();
            paginacion.Parametros["pg"] = pg.ToString();
            paginacion.Parametros["productoMarca"] = productoMarca?.ToString() ?? "0";
            paginacion.Parametros["cadenaBusqueda"] = cadenaBusqueda ?? "";

            var data = lista.Skip(paginacion.Salto).Take(paginacion.RegistrosPagina).ToList();
            this.ViewBag.Paginacion = paginacion;

            var listaMarcas = await _context.Marcas.ToListAsync();

            var productoMarcaVM = new ProductoMarcaViewModel
            {
                // Se cambió "Nombre" por "Name" para coincidir con la entidad Marca
                Marcas = new SelectList(listaMarcas, "Id", "Name"),
                Productos = data,
                ProductoMarca = productoMarca,
                CadenaBusqueda = cadenaBusqueda
            };

            return View(productoMarcaVM);
        }

        [HttpPost]
        public FileResult ExportarXLSX()
        {
            long id = Convert.ToInt32(Request.Form["id"]);
            using (XLWorkbook wb = new XLWorkbook())
            {
                var productos = _context.Productos.ToList();

                IXLWorksheet ws = wb.Worksheets.Add();

                ws.Range("A1").Value = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                ws.Range("A1").Style.Font.Bold = true;
                ws.Range("A1").Style.Font.FontSize = 14;
                ws.Range("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Range("A1:D1").Merge();
                ws.Range("A3").Value = "ID";
                ws.Range("A3").Style.Font.Bold = true;
                ws.Range("B3").Value = "NOMBRE";
                ws.Range("B3").Style.Font.Bold = true;
                ws.Range("C3").Value = "PRECIOUNITARIO";
                ws.Range("C3").Style.Font.Bold = true;
                ws.Range("D3").Value = "DESCRIPCION";
                ws.Range("D3").Style.Font.Bold = true;

                int row = 4;
                foreach (Producto item in productos)
                {
                    ws.Cell(row, 1).Value = item.Id;
                    ws.Cell(row, 2).Value = item.Nombre;
                    ws.Cell(row, 3).Value = item.PrecioUnitario;
                    ws.Cell(row, 3).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell(row, 4).Value = item.Descripcion;
                    row++;
                }
                ws.Column(1).AdjustToContents();
                ws.Column(2).AdjustToContents();
                ws.Column(3).AdjustToContents();
                ws.Column(4).AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Productos.xlsx");
                }
            }
        }

        [HttpGet]
        public async Task<IResult> GenerarVolumenVentasPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var datosVentasRaw = await _context.DetalleVentas
                .Include(dv => dv.Producto)
                .GroupBy(dv => dv.Producto != null ? dv.Producto.Nombre : "Sin Nombre")
                .Select(g => new
                {
                    Nombre = g.Key ?? "Sin Nombre",
                    Volumen = g.Sum(x => (int)x.Cantidad),
                    MontoTotal = g.Sum(x => x.Monto)
                })
                .ToListAsync();

            var datosVentas = datosVentasRaw.Select(x => new ItemVolumenVenta
            {
                Nombre = x.Nombre,
                Volumen = x.Volumen,
                MontoTotal = x.MontoTotal
            }).ToList();

            var model = new VolumenVentasModel
            {
                Titulo = "Reporte de Volumen de Ventas por Producto",
                FechaGeneracion = DateTime.Now,
                Items = datosVentas,
                GranTotal = datosVentas.Sum(x => x.MontoTotal)
            };

            var document = new VolumenVentasDocument(model);
            var pdfBytes = document.GeneratePdf();

            return Results.File(pdfBytes, "application/pdf", "VolumenVentas.pdf");
        }

    }
}