using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemCardapioController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ItemCardapioController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Lista todos os itens do cardápio")]
        public async Task<IActionResult> ListarTodos([FromQuery] PeriodoCardapio? periodo = null)
        {
            var query = _context.ItemCardapios.AsQueryable();

            if (periodo.HasValue)
                query = query.Where(i => i.Periodo == periodo.Value)
                    ;
            var itens = await _context.ItemCardapios
                .Select(i => new ItemCardapioResponseDTO(
                    i.Id,
                    i.Nome,
                    i.Descricao,
                    i.Preco,
                    i.Periodo,
                    i.ImagemUrl
                ))
                .ToListAsync();

            return Ok(itens);
        }

        
        
        [HttpGet("{id}")]
        [EndpointSummary("Busca um item do cardápio pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var item = await _context.ItemCardapios.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                item.Preco,
                item.Periodo,
                item.ImagemUrl
            );

            return Ok(response);
        }

        [HttpGet("{id}/ingredientes")]
        [EndpointSummary("Lista os ingredientes de um item do cardápio")]
        public async Task<IActionResult> ListarIngredientes(int id)
        {
            var item = await _context.ItemCardapios.FindAsync(id);
            if (item == null)
                return NotFound("Item não encontrado.");

            var ingredientes = await _context.ItemIngredientes
                .Include(ii => ii.Ingrediente)
                .Where(ii => ii.ItemCardapioId == id)
                .Select(ii => new
                {
                    ii.Ingrediente.Id,
                    ii.Ingrediente.Nome,
                    ii.Quantidade,
                    ii.UnidadeMedida
                })
                .ToListAsync();

            return Ok(ingredientes);
        }

        [HttpPost]
        [EndpointSummary("Cadastra um novo item no cardápio")]
        public async Task<IActionResult> Cadastrar(ItemCardapioRequestDTO dto)
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
                Periodo = dto.Periodo
            };

            _context.ItemCardapios.Add(item);
            await _context.SaveChangesAsync();

            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                item.Preco,
                item.Periodo,
                item.ImagemUrl
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = item.Id }, response);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Atualiza um item do cardápio")]
        public async Task<IActionResult> Atualizar(int id, ItemCardapioRequestDTO dto)
        {
            var item = await _context.ItemCardapios.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            item.Nome = dto.Nome;
            item.Descricao = dto.Descricao;
            item.Preco = dto.Preco;
            item.Periodo = dto.Periodo;

            await _context.SaveChangesAsync();

            return Ok("Item atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove um item do cardápio")]
        public async Task<IActionResult> Deletar(int id)
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
