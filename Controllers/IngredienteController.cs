using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredienteController: ControllerBase
    {
        private readonly RestauranteContext _context;

        public IngredienteController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Lista todos os ingredientes")]
        public async Task<IActionResult> ListarTodos()
        {
            var ingredientes = await _context.Ingredientes
                .Select(i => new IngredienteResponseDTO(
                    i.Id,
                    i.Nome,
                    i.Descricao,
                    i.EstoqueAtual,
                    i.EstoqueMinimo,
                    i.UnidadeMedida,
                    i.DescricaoAlergeno,
                    i.Alergeno
                ))
                .ToListAsync();

            return Ok(ingredientes);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca um ingrediente pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);

            if (ingrediente == null)
                return NotFound("Ingrediente não encontrado.");

            var response = new IngredienteResponseDTO(
                ingrediente.Id,
                ingrediente.Nome,
                ingrediente.Descricao,
                ingrediente.EstoqueAtual,
                ingrediente.EstoqueMinimo,
                ingrediente.UnidadeMedida,
                ingrediente.DescricaoAlergeno,
                ingrediente.Alergeno
            );

            return Ok(response);
        }

        [HttpPost]
        [EndpointSummary("Cadastra um novo ingrediente")]
        public async Task<IActionResult> Cadastrar(IngredienteRequestDTO dto)
        {
            var nomeExiste = await _context.Ingredientes
                .AnyAsync(i => i.Nome == dto.Nome);

            if (nomeExiste)
                return BadRequest("Já existe um ingrediente com esse nome.");

            var ingrediente = new Ingrediente
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                EstoqueAtual = dto.EstoqueAtual,
                EstoqueMinimo = dto.EstoqueMinimo,
                UnidadeMedida = dto.UnidadeMedida,
                DescricaoAlergeno = dto.DescricaoAlergeno,
                Alergeno = dto.Alergeno
            };

            _context.Ingredientes.Add(ingrediente);
            await _context.SaveChangesAsync();

            var response = new IngredienteResponseDTO(
                ingrediente.Id,
                ingrediente.Nome,
                ingrediente.Descricao,
                ingrediente.EstoqueAtual,
                ingrediente.EstoqueMinimo,
                ingrediente.UnidadeMedida,
                ingrediente.DescricaoAlergeno,
                ingrediente.Alergeno
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = ingrediente.Id }, response);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Atualiza um ingrediente")]
        public async Task<IActionResult> Atualizar(int id, IngredienteRequestDTO dto)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);

            if (ingrediente == null)
                return NotFound("Ingrediente não encontrado.");

            ingrediente.Nome = dto.Nome;
            ingrediente.Descricao = dto.Descricao;
            ingrediente.EstoqueAtual = dto.EstoqueAtual;
            ingrediente.EstoqueMinimo = dto.EstoqueMinimo;
            ingrediente.UnidadeMedida = dto.UnidadeMedida;
            ingrediente.DescricaoAlergeno = dto.DescricaoAlergeno;
            ingrediente.Alergeno = dto.Alergeno;

            await _context.SaveChangesAsync();

            return Ok("Ingrediente atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove um ingrediente")]
        public async Task<IActionResult> Deletar(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);

            if (ingrediente == null)
                return NotFound("Ingrediente não encontrado.");

            _context.Ingredientes.Remove(ingrediente);
            await _context.SaveChangesAsync();

            return Ok("Ingrediente removido com sucesso.");
        }

        [HttpPost("{ingredienteId}/vincular-item/{itemCardapioId}")]
        [EndpointSummary("Vincula um ingrediente a um item do cardápio")]
        public async Task<IActionResult> VincularItem(int ingredienteId, int itemCardapioId, ItemIngredienteRequestDTO dto)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(ingredienteId);
            if (ingrediente == null)
                return NotFound("Ingrediente não encontrado.");

            var item = await _context.ItemCardapios.FindAsync(itemCardapioId);
            if (item == null)
                return NotFound("Item do cardápio não encontrado.");

            var jaVinculado = await _context.ItemIngredientes
                .AnyAsync(ii => ii.IngredienteId == ingredienteId
                    && ii.ItemCardapioId == itemCardapioId);

            if (jaVinculado)
                return BadRequest("Ingrediente já vinculado a esse item.");

            var itemIngrediente = new ItemIngrediente
            {
                IngredienteId = ingredienteId,
                ItemCardapioId = itemCardapioId,
                Quantidade = dto.Quantidade,
                UnidadeMedida = dto.Unidade
            };

            _context.ItemIngredientes.Add(itemIngrediente);
            await _context.SaveChangesAsync();

            return Ok("Ingrediente vinculado ao item com sucesso.");
        }
    }
}

