using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventaMeCF.Models
{
    public class ProductoMarcaViewModel
    {
        public List<Producto> Productos { get; set; } = new List<Producto>();
        public SelectList? Marcas { get; set; }
        public int? ProductoMarca { get; set; }
        public string? CadenaBusqueda { get; set; }
    }
}