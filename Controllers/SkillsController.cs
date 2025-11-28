using Microsoft.AspNetCore.Mvc;
using SistemaGestionTalento.Application.Interfaces; // <-- CAMBIO: USAMOS IUnitOfWork
using SistemaGestionTalento.Domain.Entities;

namespace SistemaGestionTalento.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL: api/skills
    public class SkillsController : ControllerBase
    {
        // 1. Pedimos el IUnitOfWork completo
        private readonly IUnitOfWork _unitOfWork; // <-- CAMBIO

        public SkillsController(IUnitOfWork unitOfWork) // <-- CAMBIO: Recibe UoW
        {
            _unitOfWork = unitOfWork; // <-- CAMBIO
        }

        // GET: api/skills
        [HttpGet]
        public async Task<IActionResult> GetAllSkills()
        {
            // Usamos el UoW para acceder al repositorio Skills
            var skills = await _unitOfWork.Skills.GetAllAsync(); // <-- CAMBIO
            return Ok(skills);
        }

        // POST: api/skills
        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] Skill skill)
        {
            if (skill == null)
                return BadRequest();

            await _unitOfWork.Skills.AddAsync(skill); // <-- CAMBIO
            await _unitOfWork.CompleteAsync(); // <-- AGREGADO: Guardamos los cambios

            return CreatedAtAction(nameof(GetSkillById), new { id = skill.Id }, skill);
        }

        // GET: api/skills/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkillById(int id)
        {
            var skill = await _unitOfWork.Skills.GetByIdAsync(id); // <-- CAMBIO
            if (skill == null)
                return NotFound();
            return Ok(skill);
        }
    }
}