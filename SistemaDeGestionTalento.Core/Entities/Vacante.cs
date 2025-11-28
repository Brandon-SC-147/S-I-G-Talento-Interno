using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeGestionTalento.Core.Entities
{
    [Table("vacantes")]
    public class Vacante
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Lider")]
        [Column("lider_id")]
        public int LiderId { get; set; }
        public virtual Usuario Lider { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Proyecto { get; set; }

        [ForeignKey("UrgenciaVacante")]
        [Column("urgencia_id")]
        public int? UrgenciaId { get; set; }
        public virtual UrgenciaVacante? UrgenciaVacante { get; set; }

        [Column("fecha_inicio_requerida")]
        public DateTime? FechaInicioRequerida { get; set; }

        [StringLength(20)]
        public string Estado { get; set; } = "Abierta";

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // --- Propiedades de Navegación ---
        public virtual ICollection<VacanteSkill> VacanteSkills { get; set; } = new List<VacanteSkill>();
        public virtual ICollection<Matching> Matchings { get; set; } = new List<Matching>();
        public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    }
}