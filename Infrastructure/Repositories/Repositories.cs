using Microsoft.EntityFrameworkCore;
using SistemaGestionTalento.Domain.Entities;
using SistemaGestionTalento.Infrastructure.Persistence;

namespace SistemaGestionTalento.Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
    }

    public interface IColaboradorRepository : IGenericRepository<Colaborador> { }
    public interface ISkillRepository : IGenericRepository<Skill> { }
    public interface IVacanteRepository : IGenericRepository<Vacante>
    {
        Task<Vacante?> GetByIdWithSkillsAsync(int id);
    }

    public class ColaboradorRepository : GenericRepository<Colaborador>, IColaboradorRepository
    {
        public ColaboradorRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Colaborador>> GetAllAsync()
        {
            return await _context.Colaboradores
                .Include(c => c.Skills)
                .ToListAsync();
        }

        public override async Task<Colaborador?> GetByIdAsync(int id)
        {
            return await _context.Colaboradores
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }

    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(ApplicationDbContext context) : base(context) { }
    }

    public class VacanteRepository : GenericRepository<Vacante>, IVacanteRepository
    {
        public VacanteRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<IEnumerable<Vacante>> GetAllAsync()
        {
            return await _context.Vacantes
                .Include(v => v.Skills)
                .ToListAsync();
        }

        public async Task<Vacante?> GetByIdWithSkillsAsync(int id)
        {
            return await _context.Vacantes.Include(v => v.Skills).FirstOrDefaultAsync(v => v.Id == id);
        }

        public override async Task<Vacante?> GetByIdAsync(int id)
        {
            return await _context.Vacantes
                .Include(v => v.Skills)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
