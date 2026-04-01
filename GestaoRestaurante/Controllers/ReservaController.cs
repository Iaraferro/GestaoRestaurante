using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ReservaController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Lista todas as reservas")]
        public async Task<IActionResult> ListarTodas()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Mesa)
                .Include(r => r.Usuario)
                .Select(r => new ReservaResponseDTO(
                    r.Id,
                    r.DataHora,
                    r.MesaId,
                    r.CodigoConfirmacao
                ))
                .ToListAsync();

            return Ok(reservas);
        }

        [HttpGet("usuario/{usuarioId}")]
        [EndpointSummary("Lista as reservas de um usuário")]
        public async Task<IActionResult> ListarPorUsuario(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var reservas = await _context.Reservas
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Mesa)
                .OrderByDescending(r => r.DataHora)
                .Select(r => new ReservaResponseDTO(
                    r.Id,
                    r.DataHora,
                    r.MesaId,
                    r.CodigoConfirmacao
                ))
                .ToListAsync();

            return Ok(reservas);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca uma reserva pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Mesa)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            var response = new ReservaResponseDTO(
                reserva.Id,
                reserva.DataHora,
                reserva.MesaId,
                reserva.CodigoConfirmacao
            );

            return Ok(response);
        }

        [HttpPost]
        [EndpointSummary("Cria uma nova reserva para o almoço")]
        public async Task<IActionResult> CriarReserva(ReservaRequestDTO dto)
        {
            // Valida se o horário está dentro da janela permitida (11h-14h)
            if (dto.DataHora.Hour < 11 || dto.DataHora.Hour >= 14)
                return BadRequest("Reservas só podem ser feitas entre 19h e 22h.");

            // Valida se a reserva é feita com pelo menos 1 dia de antecedência
            if (dto.DataHora.Date <= DateTime.Today)
                return BadRequest("Reservas devem ser feitas com pelo menos 1 dia de antecedência.");

            // Valida se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            // Valida se a mesa existe
            var mesa = await _context.Mesas.FindAsync(dto.MesaId);
            if (mesa == null)
                return NotFound("Mesa não encontrada.");

            // Valida se a mesa já está reservada nesse dia
            var mesaOcupada = await _context.Reservas
                .AnyAsync(r => r.MesaId == dto.MesaId
                    && r.DataHora.Date == dto.DataHora.Date);

            if (mesaOcupada)
                return BadRequest("Mesa já reservada para essa data.");

            var reserva = new Reserva
            {
                UsuarioId = dto.UsuarioId,
                MesaId = dto.MesaId,
                DataHora = dto.DataHora,
                CodigoConfirmacao = Guid.NewGuid().ToString("N")[..8].ToUpper()
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            var response = new ReservaResponseDTO(
                reserva.Id,
                reserva.DataHora,
                reserva.MesaId,
                reserva.CodigoConfirmacao
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = reserva.Id }, response);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Cancela uma reserva")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound("Reserva não encontrada.");

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return Ok("Reserva cancelada com sucesso.");
        }
    }
}

