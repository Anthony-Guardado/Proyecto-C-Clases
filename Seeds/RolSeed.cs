using InventaMeCF.Models;
using Microsoft.EntityFrameworkCore;

namespace InventaMeCF.Seeds
{
    public class RolSeed
    {
        public RolSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Usuario" }
            );
        }
    }
}
