namespace SistemaGestionTalento.Application.DTOs
{
    public class ColaboradorCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<int> SkillIds { get; set; } = new();
    }
}
