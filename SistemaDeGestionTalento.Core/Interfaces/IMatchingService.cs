using SistemaDeGestionTalento.Core.DTOs;

namespace SistemaDeGestionTalento.Core.Interfaces
{
    public interface IMatchingService
    {
        Task<IEnumerable<MatchResultDto>> ObtenerCandidatosParaVacante(int vacanteId);
    }
}
