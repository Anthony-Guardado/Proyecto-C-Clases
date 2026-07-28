using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static InventaMeCF.Models.Usuario;

namespace InventaMeCF.Models
{
    public class RolAsignado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RolId { get; set; }

        [ForeignKey("RolId")]//Esta anotacion corresponde ala propiedad Marca

        public virtual Rol? Roles { get; set; }
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]//Esta anotacion corresponde ala propiedad Marca

        public virtual Usuario? Usuarios { get; set; }
    }
}
