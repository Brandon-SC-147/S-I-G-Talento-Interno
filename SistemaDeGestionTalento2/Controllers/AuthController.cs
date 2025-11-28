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
using System.Text.RegularExpressions;

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
            // Validar rol existente para evitar error de FK
            if (!await _context.Roles.AnyAsync(r => r.Id == request.RolId))
            {
                return BadRequest($"RolId {request.RolId} no existe. Cree el rol primero o use uno válido.");
            }

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

            // Validación estricta: comparar hash
            var hashedInput = HashPassword(request.Password);
            if (usuario.ContraseñaHash != hashedInput)
            {
                return BadRequest("Contraseña incorrecta.");
            }

            string token = CreateToken(usuario);

            return Ok(new
            {
                token,
                userId = usuario.Id,
                nombre = usuario.Nombre,
                email = usuario.Correo,
                rolId = usuario.RolId
            });
        }

        // Endpoint de mantenimiento: convierte contraseñas almacenadas en texto plano a hash SHA256
        // Criterio: si ContraseñaHash no coincide con 64 caracteres hex, se asume texto plano y se transforma.
        [HttpPost("fix-password-hashes")]
        public async Task<ActionResult> FixPasswordHashes()
        {
            var hex64 = new Regex("^[a-f0-9]{64}$", RegexOptions.IgnoreCase);
            var usuarios = await _context.Usuarios.ToListAsync();
            int updated = 0;

            foreach (var u in usuarios)
            {
                if (string.IsNullOrWhiteSpace(u.ContraseñaHash) || !hex64.IsMatch(u.ContraseñaHash))
                {
                    u.ContraseñaHash = HashPassword(u.ContraseñaHash ?? string.Empty);
                    updated++;
                }
            }

            if (updated > 0)
            {
                await _context.SaveChangesAsync();
            }

            return Ok(new { updated });
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

            var keyString = _configuration.GetSection("Jwt:Key").Value ?? throw new InvalidOperationException("JWT Key is missing");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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


