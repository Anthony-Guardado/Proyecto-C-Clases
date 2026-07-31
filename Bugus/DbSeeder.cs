using Bogus;
using InventaMeCF.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;

namespace InventaMeCF.Bugus
{
    public class DbSeeder
    {
        public static async Task SeedAsync(InventaMeCFContext context)
        {
            //evita insertar datos si ya existen

            if (context.Productos.Any())
                return;

            var faker = new Faker<Producto>("es")
                .RuleFor(x => x.Nombre, f => f.Commerce.ProductName())
                .RuleFor(x => x.Descripcion, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.PrecioUnitario, f => f.Random.Decimal(10, 500))
                .RuleFor(x => x.MarcaId, f => f.Random.Int(1, 3)); 

            var productos = faker.Generate(150);

            await context.Productos.AddRangeAsync(productos);
            await context.SaveChangesAsync();




        }

    }
}
