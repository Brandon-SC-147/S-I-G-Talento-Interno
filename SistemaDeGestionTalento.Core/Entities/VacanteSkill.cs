using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeGestionTalento.Core.Entities
{
    [Table("vacante_skills")]
    public class VacanteSkill
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Vacante")]
        [Column("vacante_id")]
        public int VacanteId { get; set; }
        public virtual Vacante Vacante { get; set; } = null!;

        [ForeignKey("Skill")]
        [Column("skill_id")]
        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; } = null!;

        [ForeignKey("NivelSkill")]
        [Column("nivel_id")]
        public int NivelId { get; set; }
        public virtual NivelSkill NivelSkill { get; set; } = null!;
    }
}