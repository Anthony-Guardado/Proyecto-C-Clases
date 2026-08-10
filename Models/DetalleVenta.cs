using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventaMeCF.Models
{
    [Table("DetalleVentas")]
    public class DetalleVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Cantidad")]
        public short Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Monto")]
        public decimal Monto { get; set; }

        public int VentaId { get; set; }

        [ForeignKey("VentaId")]
        public virtual Venta? Venta { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto? Producto { get; set; }
    }
}
