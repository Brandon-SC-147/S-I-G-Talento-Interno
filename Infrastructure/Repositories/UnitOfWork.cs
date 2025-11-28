using SistemaGestionTalento.Application.Interfaces;
using SistemaGestionTalento.Infrastructure.Persistence;

namespace SistemaGestionTalento.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IColaboradorRepository Colaboradores { get; }
        public ISkillRepository Skills { get; }
        public IVacanteRepository Vacantes { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Colaboradores = new ColaboradorRepository(context);
            Skills = new SkillRepository(context);
            Vacantes = new VacanteRepository(context);
        }

        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
    }
}
