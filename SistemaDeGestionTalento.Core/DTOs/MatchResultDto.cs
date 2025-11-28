namespace SistemaDeGestionTalento.Core.DTOs
{
    public class MatchResultDto
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double PorcentajeCoincidencia { get; set; }
        public List<string> SkillsCoincidentes { get; set; } = new List<string>();
        public List<string> SkillsFaltantes { get; set; } = new List<string>();
    }
}
