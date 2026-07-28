using InventaMeCF.Seeds;
using Microsoft.EntityFrameworkCore;


namespace InventaMeCF.Models
{
    public class InventaMeCFContext : DbContext
    {
        public InventaMeCFContext(DbContextOptions<InventaMeCFContext> options) : base(options)
        {

        }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<UnidadMedida> UnidadesMedida { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolAsignado> RolesAsignados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Forma 1 
            modelBuilder.Entity<UnidadMedida>().HasData(
                new UnidadMedida { Id = 1, Nombre = "Libra" },
                new UnidadMedida { Id = 2, Nombre = "Kilogramo" }
                );
            //Forma 2
            new MarcaSeed(modelBuilder);
            //Seed de usuarios y roles
            new UsuarioSeed(modelBuilder);
            new RolSeed(modelBuilder);
            new RolAsignadoSeed(modelBuilder);

        }

    }
}
