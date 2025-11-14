namespace SistemaGestionTalento.Domain.Entities
{
    public class Colaborador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Skills del colaborador
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
