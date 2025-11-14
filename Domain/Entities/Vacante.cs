namespace SistemaGestionTalento.Domain.Entities
{
    public class Vacante
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Skills requeridas para la vacante
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
