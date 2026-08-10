using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventaMeCF.Models
{
    [Table("Ventas")]
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("NumeroComprobante", TypeName = "varchar(25)")]
        [StringLength(25)]
        [DisplayName("Número de comprobante")]
        public string? NumeroComprobante { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("SubTotal")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("IVA")]
        public decimal Iva { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Total")]
        public decimal Total { get; set; }

        public virtual ICollection<DetalleVenta>? DetalleVentas { get; set; }
    }
}
