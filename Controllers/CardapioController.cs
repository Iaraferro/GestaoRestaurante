using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardapioController : ControllerBase
    {
        private readonly RestauranteContext _context;
        public CardapioController(RestauranteContext context) => _context = context;

        // ── PÚBLICO: retorna APENAS ativos ──────────────────────
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListarTodos()
        {
            var itens = await _context.ItemCardapios
                .Where(i => i.Ativo)                          // ← só ativos
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id, i.Nome, i.Descricao, i.Preco,
                    i.Periodo, i.ImagemUrl, i.Ativo))
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("periodo/{periodo}")]
        [AllowAnonymous]
        public async Task<IActionResult> ListarPorPeriodo(PeriodoCardapio periodo)
        {
            var itens = await _context.ItemCardapios
                .Where(i => i.Periodo == periodo && i.Ativo)  // ← só ativos
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id, i.Nome, i.Descricao, i.Preco,
                    i.Periodo, i.ImagemUrl, i.Ativo))
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("sugestao-do-dia")]
        [AllowAnonymous]
        public async Task<IActionResult> SugestaoDodia()
        {
            var sugestoes = await _context.SugestoesDoChefe
                .Include(s => s.ItemCardapio)
                .Where(s => s.Data.Date == DateTime.Today
                         && s.ItemCardapio.Ativo)             // ← só ativos
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
                        s.ItemCardapio.ImagemUrl,
                        s.ItemCardapio.Ativo)
                })
                .ToListAsync();

            if (!sugestoes.Any())
                return NotFound("Nenhuma sugestão do chefe cadastrada para hoje.");

            return Ok(sugestoes);
        }

        // ── ADMIN: lista TODOS (ativos e inativos) ──────────────
        [HttpGet("admin/todos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarTodosAdmin()
        {
            var itens = await _context.ItemCardapios
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id, i.Nome, i.Descricao, i.Preco,
                    i.Periodo, i.ImagemUrl, i.Ativo))
                .ToListAsync();

            return Ok(itens);
        }

        // ── ADMIN: criar ─────────────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CadastrarItem(ItemCardapioRequestDTO dto)
        {
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
                ImagemUrl = dto.ImagemUrl,
                Ativo = true
            };

            _context.ItemCardapios.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarTodos),
                new ItemCardapioResponseDTO(item.Id, item.Nome, item.Descricao,
                    item.Preco, item.Periodo, item.ImagemUrl, item.Ativo));
        }

        // ── ADMIN: editar ────────────────────────────────────────
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarItem(int id, [FromBody] ItemCardapioRequestDTO dto)
        {
            var item = await _context.ItemCardapios.FindAsync(id);
            if (item == null) return NotFound("Item não encontrado.");

            if (item.Periodo != dto.Periodo)
            {
                var totalNovoPeriodo = await _context.ItemCardapios
                    .CountAsync(i => i.Periodo == dto.Periodo);
                if (totalNovoPeriodo >= 20)
                    return BadRequest($"O cardápio de {dto.Periodo} já atingiu o limite de 20 itens.");
            }

            item.Nome = dto.Nome;
            item.Descricao = dto.Descricao;
            item.Preco = dto.Preco;
            item.Periodo = dto.Periodo;
            item.ImagemUrl = dto.ImagemUrl;

            _context.ItemCardapios.Update(item);
            await _context.SaveChangesAsync();

            return Ok(new ItemCardapioResponseDTO(item.Id, item.Nome, item.Descricao,
                item.Preco, item.Periodo, item.ImagemUrl, item.Ativo));
        }

        // ── ADMIN: alternar Ativo/Inativo (soft delete) ──────────
        [HttpPut("{id}/alternar-status")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AlternarStatus(int id)
        {
            var item = await _context.ItemCardapios.FindAsync(id);
            if (item == null) return NotFound("Item não encontrado.");

            item.Ativo = !item.Ativo;  // ← toggle
            await _context.SaveChangesAsync();

            return Ok(new
            {
                id = item.Id,
                ativo = item.Ativo,
                mensagem = item.Ativo ? "Prato ativado com sucesso." : "Prato desativado com sucesso."
            });
        }

        // ── ADMIN: ativar ou desativar TODOS de uma vez ───────────
        [HttpPut("alternar-status-todos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AlternarStatusTodos([FromQuery] string ativo) // CORREÇÃO AQUI: 'string ativo'
        {
            // CORREÇÃO AQUI: TryParse
            if (!bool.TryParse(ativo, out bool ativoValor))
                return BadRequest("Parâmetro 'ativo' deve ser 'true' ou 'false'.");

            var itens = await _context.ItemCardapios.ToListAsync();

            if (!itens.Any())
                return NotFound("Nenhum item no cardápio.");

            foreach (var item in itens)
                item.Ativo = ativoValor;

            await _context.SaveChangesAsync();

            var acao = ativoValor ? "ativados" : "desativados";
            return Ok(new
            {
                total = itens.Count,
                mensagem = $"Todos os {itens.Count} pratos foram {acao} com sucesso."
            });
        }

        // ── ADMIN: excluir permanentemente ───────────────────────
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarItem(int id)
        {
            var item = await _context.ItemCardapios.FindAsync(id);
            if (item == null) return NotFound("Item não encontrado.");

            _context.ItemCardapios.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removido permanentemente do cardápio.");
        }
    }
}
