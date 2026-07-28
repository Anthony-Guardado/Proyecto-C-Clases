using InventaMeCF.Models;
using Microsoft.EntityFrameworkCore;

namespace InventaMeCF.Seeds
{
    public class RolAsignadoSeed
    {
        public RolAsignadoSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolAsignado>().HasData(
                new RolAsignado { Id = 1, UsuarioId = 1, RolId = 1 },
                new RolAsignado { Id = 2, UsuarioId = 2, RolId = 2 }
            );
        }
    }
}
