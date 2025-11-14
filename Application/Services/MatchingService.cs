using Microsoft.EntityFrameworkCore;
using SistemaGestionTalento.Application.Interfaces;
using SistemaGestionTalento.Application.Interfaces.Services;
using SistemaGestionTalento.Domain.Entities;

namespace SistemaGestionTalento.Application.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IUnitOfWork _uow;

        public MatchingService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Colaborador>> FindBestCandidatesForVacante(int vacanteId)
        {
            var vacante = await _uow.Vacantes.GetByIdWithSkillsAsync(vacanteId);
            if (vacante == null || vacante.Skills.Count == 0) return Enumerable.Empty<Colaborador>();

            var colaboradores = await _uow.Colaboradores.GetAllAsync();

            var requeridas = vacante.Skills.Select(s => s.Id).ToHashSet();

            var ranking = colaboradores
                .Select(c => new
                {
                    Colaborador = c,
                    Puntos = c.Skills.Count(s => requeridas.Contains(s.Id))
                })
                .Where(x => x.Puntos > 0)
                .OrderByDescending(x => x.Puntos)
                .Select(x => x.Colaborador);

            return ranking;
        }
    }
}
