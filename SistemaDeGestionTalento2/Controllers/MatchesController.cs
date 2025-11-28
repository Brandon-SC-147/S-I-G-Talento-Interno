using Microsoft.AspNetCore.Mvc;
using SistemaDeGestionTalento.Core.DTOs;
using SistemaDeGestionTalento.Core.Interfaces;

namespace SistemaDeGestionTalento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchesController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        [HttpGet("vacante/{vacanteId}")]
        public async Task<ActionResult<IEnumerable<MatchResultDto>>> GetMatchesParaVacante(int vacanteId)
        {
            var matches = await _matchingService.ObtenerCandidatosParaVacante(vacanteId);
            
            if (!matches.Any())
            {
                return NotFound(new { message = "No se encontraron candidatos compatibles o la vacante no existe/no tiene skills requeridos." });
            }

            return Ok(matches);
        }
    }
}
