using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardapioController : ControllerBase
    {
        private readonly RestauranteContext _context;


        public CardapioController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var itens = await _context.ItemCardapios
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id,
                    i.Nome,
                    i.Descricao,
                    i.Preco,
                    i.Periodo
                ))
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("periodo/{periodo}")]
        public async Task<IActionResult> ListarPorPeriodo(PeriodoCardapio periodo)
        {
            var itens = await _context.ItemCardapios
                .Where(i => i.Periodo == periodo)
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id,
                    i.Nome,
                    i.Descricao,
                    i.Preco,
                    i.Periodo
                ))
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("sugestao-do-dia")]
        public async Task<IActionResult> SugestaoDodia()
        {
            var sugestoes = await _context.SugestoesDoChefe
                .Include(s => s.ItemCardapio)
                .Where(s => s.Data.Date == DateTime.Today)
                .Select(s => new
                {
                    s.Id,
                    s.Periodo,
                    Item = new ItemCardapioResponseDTO(
                        s.ItemCardapio.Id,
                        s.ItemCardapio.Nome,
                        s.ItemCardapio.Descricao,
                        s.ItemCardapio.Preco * 0.80m,
                        s.ItemCardapio.Periodo
                    )
                })
                .ToListAsync();

            if (!sugestoes.Any())
                return NotFound("Nenhuma sugestão do chefe cadastrada para hoje.");

            return Ok(sugestoes);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarItem(ItemCardapioRequestDTO dto)
        {
            // Valida limite de 20 itens por período
            var totalPeriodo = await _context.ItemCardapios
                .CountAsync(i => i.Periodo == dto.Periodo);

            if (totalPeriodo >= 20)
                return BadRequest($"O cardápio de {dto.Periodo} já atingiu o limite de 20 itens.");

            var item = new ItemCardapio
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = (double)dto.Preco,
                Periodo = dto.Periodo
            };

            _context.ItemCardapios.Add(item);
            await _context.SaveChangesAsync();

            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                dto.Preco,
                item.Periodo
            );

            return CreatedAtAction(nameof(ListarTodos), response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarItem(int id)
        {
            var item = await _context.ItemCardapios.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            _context.ItemCardapios.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removido do cardápio.");
        }
    }
}

