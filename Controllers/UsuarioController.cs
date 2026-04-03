using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly RestauranteContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioController(RestauranteContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")] // ← apenas admin vê todos os usuários
        [EndpointSummary("Lista todos os usuários")]
        public async Task<IActionResult> ListarTodos()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioResponseDTO(u.Id, u.UserName, u.Perfil))
                .ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize]
        [EndpointSummary("Busca um usuário pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuário não encontrado.");
            return Ok(new UsuarioResponseDTO(usuario.Id, usuario.UserName, usuario.Perfil));
        }

        [HttpPost("cadastrar")]
        [AllowAnonymous] // ← público, qualquer um pode se cadastrar
        [EndpointSummary("Cadastra um novo usuário")]
        public async Task<IActionResult> Cadastrar(UsuarioRequestDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email já cadastrado.");

            if (await _context.Usuarios.AnyAsync(u => u.UserName == dto.UserName))
                return BadRequest("Nome de usuário já está em uso.");

            var usuario = new Usuario
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHasher),
                Perfil = PerfilUsuario.Usuario,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.Id },
                new UsuarioResponseDTO(usuario.Id, usuario.UserName, usuario.Perfil));
        }

        [HttpPost("login")]
        [AllowAnonymous] // ← público
        [EndpointSummary("Realiza login e retorna o token JWT")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHasher, usuario.Password))
                return Unauthorized("Senha incorreta.");

            // ← NOVO: Gera o token JWT com a Role do usuário
            var chave = _configuration["Jwt:Chave"] ?? "ChaveSecretaRestaurante2026SuperSegura!";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString()) // "Administrador" ou "Usuario"
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(chave)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                usuario = new UsuarioResponseDTO(usuario.Id, usuario.UserName, usuario.Perfil)
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        [EndpointSummary("Remove um usuário")]
        public async Task<IActionResult> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuário não encontrado.");
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok("Usuário removido com sucesso.");
        }
    }
}