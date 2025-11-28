using SistemaGestionTalento.Infrastructure.Repositories;

namespace SistemaGestionTalento.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IColaboradorRepository Colaboradores { get; }
        ISkillRepository Skills { get; }
        IVacanteRepository Vacantes { get; }
        Task<int> CompleteAsync();
    }
}
