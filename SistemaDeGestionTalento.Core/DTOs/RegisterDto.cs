namespace SistemaDeGestionTalento.Core.DTOs
{
    public class RegisterDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public int RolId { get; set; }
    }
}
