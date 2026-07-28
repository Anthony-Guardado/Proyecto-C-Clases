using Microsoft.EntityFrameworkCore;
using InventaMeCF.Models;


namespace InventaMeCF.Seeds
{
    public class MarcaSeed
    {
        public MarcaSeed(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Marca>().HasData(
                new Marca { Id = 1, Name = "Marca A" },
                new Marca { Id = 2, Name = "Marca B" },
                new Marca { Id = 3, Name = "Marca C" }
                );
        }

    }
}
