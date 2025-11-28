namespace SistemaGestionTalento.Domain.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Relación muchos a muchos con Colaborador y Vacante
        public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
        public ICollection<Vacante> Vacantes { get; set; } = new List<Vacante>();
    }
}
