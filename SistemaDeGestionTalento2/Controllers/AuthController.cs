using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaDeGestionTalento.Core.DTOs;
using SistemaDeGestionTalento.Core.Entities;
using SistemaDeGestionTalento.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SistemaDeGestionTalento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SgiDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SgiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(RegisterDto request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Correo == request.Email))
            {
                return BadRequest("El usuario ya existe.");
            }

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Correo = request.Email,
                ContraseñaHash = HashPassword(request.Password),
                PuestoActual = request.Puesto,
                RolId = request.RolId,
                FechaCreacion = DateTime.Now,
                Estado = "Activo"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginDto request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Email);

            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            if (usuario.ContraseñaHash != HashPassword(request.Password))
            {
                return BadRequest("Contraseña incorrecta.");
            }

            string token = CreateToken(usuario);

            return Ok(new
            {
                token,
                user = new { id = usuario.Id, email = usuario.Correo, nombre = usuario.Nombre, apellido = usuario.Apellido, rolId = usuario.RolId }
            });
        }

        [HttpGet("me")]
        public async Task<ActionResult<object>> Me()
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }

            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(sub) || !int.TryParse(sub, out var userId))
            {
                return Unauthorized();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);
            if (usuario == null)
            {
                return Unauthorized();
            }

            return Ok(new { id = usuario.Id, email = usuario.Correo, nombre = usuario.Nombre, apellido = usuario.Apellido, rolId = usuario.RolId });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private string CreateToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.RolId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value ?? throw new InvalidOperationException("JWT Key is missing")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value ?? throw new InvalidOperationException("JWT Issuer is missing"),
                audience: _configuration.GetSection("Jwt:Audience").Value ?? throw new InvalidOperationException("JWT Audience is missing"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
