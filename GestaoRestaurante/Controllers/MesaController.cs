using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesaController: ControllerBase
    {
        private readonly RestauranteContext _context;

        public MesaController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Lista todas as mesas")]
        public async Task<IActionResult> ListarTodas()
        {
            var mesas = await _context.Mesas
                .Select(m => new MesaResponseDTO(
                    m.Id,
                    m.Numero,
                    m.Capacidade
                ))
                .ToListAsync();

            return Ok(mesas);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca uma mesa pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound("Mesa não encontrada.");

            var response = new MesaResponseDTO(
                mesa.Id,
                mesa.Numero,
                mesa.Capacidade
            );

            return Ok(response);
        }

        [HttpGet("disponiveis")]
        [EndpointSummary("Lista mesas disponíveis em uma data")]
        public async Task<IActionResult> ListarDisponiveis([FromQuery] DateTime data)
        {
            var mesasOcupadas = await _context.Reservas
                .Where(r => r.DataHora.Date == data.Date)
                .Select(r => r.MesaId)
                .ToListAsync();

            var mesasDisponiveis = await _context.Mesas
                .Where(m => !mesasOcupadas.Contains(m.Id))
                .Select(m => new MesaResponseDTO(
                    m.Id,
                    m.Numero,
                    m.Capacidade
                ))
                .ToListAsync();

            return Ok(mesasDisponiveis);
        }

        [HttpPost]
        [EndpointSummary("Cadastra uma nova mesa")]
        public async Task<IActionResult> CadastrarMesa(MesaRequestDTO dto)
        {
            var numeroExiste = await _context.Mesas
                .AnyAsync(m => m.Numero == dto.Numero);

            if (numeroExiste)
                return BadRequest("Já existe uma mesa com esse número.");

            var mesa = new Mesa
            {
                Numero = dto.Numero,
                Capacidade = dto.Capacidade
            };

            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();

            var response = new MesaResponseDTO(
                mesa.Id,
                mesa.Numero,
                mesa.Capacidade
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = mesa.Id }, response);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove uma mesa")]
        public async Task<IActionResult> Deletar(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
                return NotFound("Mesa não encontrada.");

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return Ok("Mesa removida com sucesso.");
        }
    }
}

