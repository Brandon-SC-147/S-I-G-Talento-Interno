using Microsoft.EntityFrameworkCore;
using SistemaDeGestionTalento.Core.DTOs;
using SistemaDeGestionTalento.Core.Interfaces;
using SistemaDeGestionTalento.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaDeGestionTalento.Infrastructure.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly SgiDbContext _context;

        public MatchingService(SgiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchResultDto>> ObtenerCandidatosParaVacante(int vacanteId)
        {
            return new List<MatchResultDto>();
        }
    }
}
