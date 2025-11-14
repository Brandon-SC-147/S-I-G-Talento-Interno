// Archivo: DTOs/CrearVacanteDto.cs

using System.ComponentModel.DataAnnotations;

namespace SistemaDeGestionTalento.DTOs
{
    // DTO para crear una nueva vacante
    public class CrearVacanteDto
    {
        [Required]
        public int LiderId { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [StringLength(150)]
        public string? Proyecto { get; set; }

        public int? UrgenciaId { get; set; }
        public DateTime? FechaInicioRequerida { get; set; }
    }
}