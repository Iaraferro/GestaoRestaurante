using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemIngredienteController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ItemIngredienteController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet("item/{itemCardapioId}")]
        [EndpointSummary("Lista os ingredientes de um item do cardápio")]
        public async Task<IActionResult> ListarPorItem(int itemCardapioId)
        {
            var item = await _context.ItemCardapios.FindAsync(itemCardapioId);
            if (item == null)
                return NotFound("Item do cardápio não encontrado.");

            var ingredientes = await _context.ItemIngredientes
                .Include(ii => ii.Ingrediente)
                .Where(ii => ii.ItemCardapioId == itemCardapioId)
                .Select(ii => new ItemIngredienteResponseDTO(
                    ii.Id,
                    ii.Quantidade,
                    ii.UnidadeMedida
                ))
                .ToListAsync();

            return Ok(ingredientes);
        }

        [HttpPost]
        [EndpointSummary("Vincula um ingrediente a um item do cardápio")]
        public async Task<IActionResult> Vincular(ItemIngredienteRequestDTO dto)
        {
            var item = await _context.ItemCardapios.FindAsync(dto.ItemCardapioId);
            if (item == null)
                return NotFound("Item do cardápio não encontrado.");

            var ingrediente = await _context.Ingredientes.FindAsync(dto.IngredienteId);
            if (ingrediente == null)
                return NotFound("Ingrediente não encontrado.");

            var jaExiste = await _context.ItemIngredientes
                .AnyAsync(ii => ii.ItemCardapioId == dto.ItemCardapioId
                    && ii.IngredienteId == dto.IngredienteId);

            if (jaExiste)
                return BadRequest("Esse ingrediente já está vinculado a esse item.");

            var itemIngrediente = new ItemIngrediente
            {
                ItemCardapioId = dto.ItemCardapioId,
                IngredienteId = dto.IngredienteId,
                Quantidade = dto.Quantidade,
                UnidadeMedida = dto.Unidade
            };

            _context.ItemIngredientes.Add(itemIngrediente);
            await _context.SaveChangesAsync();

            var response = new ItemIngredienteResponseDTO(
                itemIngrediente.Id,
                itemIngrediente.Quantidade,
                itemIngrediente.UnidadeMedida
            );

            return CreatedAtAction(nameof(ListarPorItem), new { itemCardapioId = dto.ItemCardapioId }, response);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Desvincula um ingrediente de um item do cardápio")]
        public async Task<IActionResult> Desvincular(int id)
        {
            var itemIngrediente = await _context.ItemIngredientes.FindAsync(id);

            if (itemIngrediente == null)
                return NotFound("Vínculo não encontrado.");

            _context.ItemIngredientes.Remove(itemIngrediente);
            await _context.SaveChangesAsync();

            return Ok("Ingrediente desvinculado com sucesso.");
        }
    }
}
