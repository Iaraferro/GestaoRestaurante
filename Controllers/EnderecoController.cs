using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public EnderecoController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet("usuario/{usuarioId}")]
        [EndpointSummary("Lista todos os endereços de um usuário")]
        public async Task<IActionResult> ListarPorUsuario(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var enderecos = await _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId)
                .Select(e => new EnderecoResponseDTO(
                    e.Id,
                    e.Rua,
                    e.Numero,
                    e.Bairro,
                    e.Cidade,
                    e.Estado
                ))
                .ToListAsync();

            return Ok(enderecos);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca um endereço pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
                return NotFound("Endereço não encontrado.");

            var response = new EnderecoResponseDTO(
                endereco.Id,
                endereco.Rua,
                endereco.Numero,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado
            );

            return Ok(response);
        }

        [HttpPost]
        [EndpointSummary("Cadastra um novo endereço para o usuário")]
        public async Task<IActionResult> Cadastrar(int usuarioId, EnderecoRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var endereco = new Endereco
            {
                Rua = dto.Rua,
                Numero = dto.Numero,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                UsuarioId = dto.UsuarioId
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var response = new EnderecoResponseDTO(
                endereco.Id,
                endereco.Rua,
                endereco.Numero,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = endereco.Id }, response);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Atualiza um endereço existente")]
        public async Task<IActionResult> Atualizar(int id, EnderecoRequestDTO dto)
        {
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
                return NotFound("Endereço não encontrado.");

            endereco.Rua = dto.Rua;
            endereco.Numero = dto.Numero;
            endereco.Bairro = dto.Bairro;
            endereco.Cidade = dto.Cidade;
            endereco.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            return Ok("Endereço atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove um endereço")]
        public async Task<IActionResult> Deletar(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
                return NotFound("Endereço não encontrado.");

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return Ok("Endereço removido com sucesso.");
        }
    }
}
