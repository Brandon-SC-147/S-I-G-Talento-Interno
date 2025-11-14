using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionTalento.Application.Interfaces; // <-- CAMBIO
using SistemaGestionTalento.Domain.Entities;
using SistemaGestionTalento.Application.DTOs;

namespace SistemaGestionTalento.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColaboradoresController : ControllerBase
    {
        // Pedimos el UnitOfWork completo
        private readonly IUnitOfWork _unitOfWork; // <-- CAMBIO

        public ColaboradoresController(IUnitOfWork unitOfWork) // <-- CAMBIO
        {
            _unitOfWork = unitOfWork; // <-- CAMBIO
        }

        // GET: api/colaboradores
        [HttpGet]
        public async Task<IActionResult> GetAllColaboradores()
        {
            // Usamos el UoW para acceder al repositorio
            var colaboradores = await _unitOfWork.Colaboradores.GetAllAsync(); // <-- CAMBIO
            return Ok(colaboradores);
        }

        // GET: api/colaboradores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetColaboradorById(int id)
        {
            var colaborador = await _unitOfWork.Colaboradores.GetByIdAsync(id); // <-- CAMBIO
            if (colaborador == null)
            {
                return NotFound();
            }
            return Ok(colaborador);
        }

        // POST: api/colaboradores
        [HttpPost]
        public async Task<IActionResult> CreateColaborador([FromBody] ColaboradorCreateDto input)
        {
            if (input == null)
            {
                return BadRequest();
            }

            var colaborador = new Colaborador
            {
                Nombre = input.Nombre,
                Email = input.Email,
            };

            if (input.SkillIds?.Count > 0)
            {
                // Obtener las skills existentes por ids
                var allSkills = await _unitOfWork.Skills.GetAllAsync();
                var skills = allSkills.Where(s => input.SkillIds.Contains(s.Id)).ToList();
                foreach (var s in skills)
                {
                    colaborador.Skills.Add(s);
                }
            }

            await _unitOfWork.Colaboradores.AddAsync(colaborador); // <-- CAMBIO

            // ¡EL PASO CLAVE! Guardamos todos los cambios.
            await _unitOfWork.CompleteAsync(); // <-- CAMBIO

            return CreatedAtAction(nameof(GetColaboradorById), new { id = colaborador.Id }, colaborador);
        }
    }
}