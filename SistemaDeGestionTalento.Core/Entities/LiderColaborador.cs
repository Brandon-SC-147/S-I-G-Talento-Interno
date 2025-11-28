using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeGestionTalento.Core.Entities
{
    [Table("lider_colaborador")]
    public class LiderColaborador
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Lider")]
        public int LiderId { get; set; }
        public virtual Usuario Lider { get; set; } = null!;

        [ForeignKey("Colaborador")]
        public int ColaboradorId { get; set; }
        public virtual Usuario Colaborador { get; set; } = null!;
    }
}