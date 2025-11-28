using Microsoft.AspNetCore.Mvc;
using SistemaGestionTalento.Application.Interfaces; // <-- CAMBIO
using SistemaGestionTalento.Domain.Entities;

namespace SistemaGestionTalento.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacantesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork; // <-- CAMBIO

        public VacantesController(IUnitOfWork unitOfWork) // <-- CAMBIO
        {
            _unitOfWork = unitOfWork; // <-- CAMBIO
        }

        // GET: api/vacantes
        [HttpGet]
        public async Task<IActionResult> GetAllVacantes()
        {
            var vacantes = await _unitOfWork.Vacantes.GetAllAsync(); // <-- CAMBIO
            return Ok(vacantes);
        }

        // GET: api/vacantes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVacanteById(int id)
        {
            var vacante = await _unitOfWork.Vacantes.GetByIdWithSkillsAsync(id); // <-- CAMBIO
            if (vacante == null)
                return NotFound();

            return Ok(vacante);
        }

        // POST: api/vacantes
        [HttpPost]
        public async Task<IActionResult> CreateVacante([FromBody] Vacante vacante)
        {
            if (vacante == null)
                return BadRequest();

            await _unitOfWork.Vacantes.AddAsync(vacante); // <-- CAMBIO
            await _unitOfWork.CompleteAsync(); // <-- CAMBIO

            return CreatedAtAction(nameof(GetVacanteById), new { id = vacante.Id }, vacante);
        }
    }
}