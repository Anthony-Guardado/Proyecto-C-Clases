using InventaMeCF.Models;
using Microsoft.EntityFrameworkCore;

namespace InventaMeCF.Seeds
{
    public class UsuarioSeed
    {
        public UsuarioSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Anthony Stiven",
                    Correo = "anthony@example.com",
                    Clave = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef"
                },
                 new Usuario
                 {
                     Id = 2,
                     Nombre = "Anthony Menjivar",
                     Correo = "menjivar@example.com",
                     Clave = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef"
                 },
                  new Usuario
                  {
                      Id = 3,
                      Nombre = "Anthony Guardado",
                      Correo = "guardado@example.com",
                      Clave = "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef"
                  }


                );
        }
    }
}
