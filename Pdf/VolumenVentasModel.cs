using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventaMeCF.Pdf
{
    [NotMapped]
    public class ItemVolumenVenta
    {
        public string Nombre { get; set; } = string.Empty;

        public string ProductoNombre
        {
            get => Nombre;
            set => Nombre = value;
        }

        public int Volumen { get; set; }

        public int CantidadTotal
        {
            get => Volumen;
            set => Volumen = value;
        }

        public decimal MontoTotal { get; set; }
    }

    [NotMapped]
    public class VolumenVentasModel
    {
        // Propiedades de la clase principal
        public string Nombre { get; set; } = string.Empty;
        public int Volumen { get; set; }

        public string Titulo { get; set; } = "Reporte de Volumen de Ventas";
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public List<ItemVolumenVenta> Items { get; set; } = new List<ItemVolumenVenta>();
        public decimal GranTotal { get; set; }
    }
}
