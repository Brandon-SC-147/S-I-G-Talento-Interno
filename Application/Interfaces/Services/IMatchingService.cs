using SistemaGestionTalento.Domain.Entities;

namespace SistemaGestionTalento.Application.Interfaces.Services
{
    public interface IMatchingService
    {
        Task<IEnumerable<Colaborador>> FindBestCandidatesForVacante(int vacanteId);
    }
}
