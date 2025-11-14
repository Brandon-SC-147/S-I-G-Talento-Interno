using Microsoft.AspNetCore.Mvc;
using SistemaGestionTalento.Application.Interfaces.Services; // Para el Matching Service

namespace SistemaGestionTalento.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL: api/matching
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        // GET: api/matching/vacante/5
        [HttpGet("vacante/{vacanteId}")]
        public async Task<IActionResult> FindMatchesForVacante(int vacanteId)
        {
            if (vacanteId <= 0)
                return BadRequest("El ID de la vacante no es válido.");

            var candidatos = await _matchingService.FindBestCandidatesForVacante(vacanteId);

            if (candidatos == null || !candidatos.Any())
                return NotFound("No se encontraron candidatos compatibles para esta vacante.");

            return Ok(candidatos); // Devuelve la lista ordenada de colaboradores
        }
    }
}