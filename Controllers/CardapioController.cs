using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
        public async Task<IActionResult> ListarTodos()
        {
            var itens = await _context.ItemCardapios
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id,
                    i.Nome,
                    i.Descricao,
                    i.Preco,
                    i.Periodo,
                    i.ImagemUrl // ← Corrigido aqui
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
                    i.Periodo,
                    i.ImagemUrl // ← Corrigido aqui
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
                        s.ItemCardapio.Periodo,
                        s.ItemCardapio.ImagemUrl // ← Corrigido aqui
                    )
                })
                .ToListAsync();

            if (!sugestoes.Any())
                return NotFound("Nenhuma sugestão do chefe cadastrada para hoje.");

            return Ok(sugestoes);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
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
                Preco = dto.Preco,
                Periodo = dto.Periodo,
                ImagemUrl = dto.ImagemUrl // ← Garantindo que a imagem seja salva no banco
            };

            _context.ItemCardapios.Add(item);
            await _context.SaveChangesAsync();

            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                item.Preco,
                item.Periodo,
                item.ImagemUrl // ← Corrigido aqui
            );

            return CreatedAtAction(nameof(ListarTodos), response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarItem(int id, [FromBody] ItemCardapioRequestDTO dto)
        {
            var item = await _context.ItemCardapios.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            // Se o período do item estiver sendo alterado, precisamos validar a regra de limite (20 itens)
            if (item.Periodo != dto.Periodo)
            {
                var totalNovoPeriodo = await _context.ItemCardapios
                    .CountAsync(i => i.Periodo == dto.Periodo);

                if (totalNovoPeriodo >= 20)
                    return BadRequest($"O cardápio de {dto.Periodo} já atingiu o limite de 20 itens. Não é possível mover este item para lá.");
            }

            // Atualiza os dados do item
            item.Nome = dto.Nome;
            item.Descricao = dto.Descricao;
            item.Preco = dto.Preco; // ← Corrigido o erro CS0103 aqui
            item.Periodo = dto.Periodo;
            item.ImagemUrl = dto.ImagemUrl; // ← Atualizando a imagem no banco

            _context.ItemCardapios.Update(item);
            await _context.SaveChangesAsync();

            // Retorna o DTO atualizado para o frontend
            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                item.Preco,
                item.Periodo,
                item.ImagemUrl // ← Corrigido aqui
            );

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
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
