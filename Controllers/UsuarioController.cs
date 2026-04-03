using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public UsuarioController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Lista todos os usuários")]
        public async Task<IActionResult> ListarTodos()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioResponseDTO(
                    u.Id,
                    u.UserName,
                    u.Perfil
                ))
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca um usuário pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var response = new UsuarioResponseDTO(
                usuario.Id,
                usuario.UserName,
                usuario.Perfil
            );

            return Ok(response);
        }

        [HttpPost("cadastrar")]
        [EndpointSummary("Cadastra um novo usuário")]
        public async Task<IActionResult> Cadastrar(UsuarioRequestDTO dto)
        {
            // Verifica se o email já está cadastrado
            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExiste)
                return BadRequest("Email já cadastrado.");

            // Verifica se o username já está em uso
            var usernameExiste = await _context.Usuarios
                .AnyAsync(u => u.UserName == dto.UserName);

            if (usernameExiste)
                return BadRequest("Nome de usuário já está em uso.");

            var usuario = new Usuario
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHasher),
                Perfil = PerfilUsuario.Usuario
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var response = new UsuarioResponseDTO(
                usuario.Id,
                usuario.UserName,
                usuario.Perfil
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.Id }, response);
        }

        [HttpPost("login")]
        [EndpointSummary("Realiza login do usuário")]
        public async Task<IActionResult> Login(UsuarioRequestDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHasher, usuario.Password))
                return Unauthorized("Senha incorreta.");

            var response = new UsuarioResponseDTO(
                usuario.Id,
                usuario.UserName,
                usuario.Perfil
            );

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove um usuário")]
        public async Task<IActionResult> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário removido com sucesso.");
        }
    }
}
