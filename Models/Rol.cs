using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventaMeCF.Models
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Nombre", TypeName = "varchar(100)")]
        [DisplayName("Nombre del usuario")]
        [Required(ErrorMessage = "El nombre del usuario es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre del usuario debe tener una longitud mínima de 3 caracteres y como máximo 100",
        MinimumLength = 3)]
        public string? Nombre { get; set; }
        public virtual ICollection<RolAsignado> RolesAsignados { get; set; }
    }
}
