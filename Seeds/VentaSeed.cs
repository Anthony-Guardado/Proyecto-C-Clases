using Microsoft.EntityFrameworkCore;
using InventaMeCF.Models;

namespace InventaMeCF.Seeds
{
    public class VentaSeed
    {
        public VentaSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venta>().HasData(
                new Venta
                {
                    Id = 1,
                    NumeroComprobante = "FAC-0001",
                    Fecha = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    SubTotal = 100.00m,
                    Iva = 13.00m,
                    Total = 113.00m
                },
                new Venta
                {
                    Id = 2,
                    NumeroComprobante = "FAC-0002",
                    Fecha = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc),
                    SubTotal = 250.00m,
                    Iva = 32.50m,
                    Total = 282.50m
                },
                new Venta
                {
                    Id = 3,
                    NumeroComprobante = "FAC-0003",
                    Fecha = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                    SubTotal = 500.00m,
                    Iva = 65.00m,
                    Total = 565.00m
                }
            );
        }
    }
}
