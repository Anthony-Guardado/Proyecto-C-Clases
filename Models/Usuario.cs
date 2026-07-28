using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventaMeCF.Models
{
    public class Usuario
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

            [Column("Correo", TypeName = "varchar(80)")]
            [Required(ErrorMessage = "El correo es requerido.")]
            [StringLength(80, ErrorMessage = "El correo debe tener entre 3 y 80 caracteres.", MinimumLength = 3)]
            public string Correo { get; set; } = null!;

            [Column("Clave", TypeName = "varchar(64)")]
            [DisplayName("Contraseña")]
            [Required(ErrorMessage = "La contraseña es requerida.")]
            [StringLength(64, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.", MinimumLength = 64)]
            public string Clave { get; set; } = null!;

            // Propiedad de navegación inversa (Un usuario puede tener múltiples roles asignados)
            
            public virtual ICollection<RolAsignado> RolesAsignados { get; set; }

        }
}
