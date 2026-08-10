using InventaMeCF.Pdf;
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

        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeds
            modelBuilder.Entity<UnidadMedida>().HasData(
                new UnidadMedida { Id = 1, Nombre = "Libra" },
                new UnidadMedida { Id = 2, Nombre = "Kilogramo" }
            );

            new MarcaSeed(modelBuilder);
            new UsuarioSeed(modelBuilder);
            new RolSeed(modelBuilder);
            new RolAsignadoSeed(modelBuilder);
            new VentaSeed(modelBuilder);
            new DetalleVentaSeed(modelBuilder);

            /* Configuración de relaciones
            modelBuilder.Entity<RolAsignado>()
                .HasOne(ra => ra.Usuario)
                .WithMany(u => u.RolesAsignados)
                .HasForeignKey(ra => ra.UsuarioId);

            modelBuilder.Entity<RolAsignado>()
                .HasOne(ra => ra.Rol)
                .WithMany(r => r.RolesAsignados)
                .HasForeignKey(ra => ra.RolId); */
        }


    }
}
