using Microsoft.EntityFrameworkCore;
using InventaMeCF.Models;

namespace InventaMeCF.Seeds
{
    public class DetalleVentaSeed
    {
        public DetalleVentaSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleVenta>().HasData(
                new DetalleVenta
                {
                    Id = 1,
                    Cantidad = 2,
                    PrecioUnitario = 50.00m,
                    Monto = 100.00m,
                    VentaId = 1,
                    ProductoId = 12
                },
                new DetalleVenta
                {
                    Id = 2,
                    Cantidad = 5,
                    PrecioUnitario = 30.00m,
                    Monto = 150.00m,
                    VentaId = 2,
                    ProductoId = 13
                },
                new DetalleVenta
                {
                    Id = 3,
                    Cantidad = 2,
                    PrecioUnitario = 50.00m,
                    Monto = 100.00m,
                    VentaId = 2,
                    ProductoId = 14
                },
                new DetalleVenta
                {
                    Id = 4,
                    Cantidad = 10,
                    PrecioUnitario = 50.00m,
                    Monto = 500.00m,
                    VentaId = 3,
                    ProductoId = 15
                }
            );
        }
    }
}
