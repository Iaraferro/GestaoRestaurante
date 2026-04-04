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
    public class ItemCardapioController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ItemCardapioController(RestauranteContext context)
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
                    i.Periodo,
                    i.ImagemUrl,
                    i.Ativo // ← CORREÇÃO AQUI
                ))
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("{id}")]
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
                item.ImagemUrl,
                item.Ativo // ← CORREÇÃO AQUI
            );

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Criar(ItemCardapioRequestDTO dto)
        {
            var item = new ItemCardapio
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                Periodo = dto.Periodo,
                ImagemUrl = dto.ImagemUrl,
                Ativo = true // Por padrão já nasce ativo
            };

            _context.ItemCardapios.Add(item);
            await _context.SaveChangesAsync();

            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                item.Preco,
                item.Periodo,
                item.ImagemUrl,
                item.Ativo // ← CORREÇÃO AQUI
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = item.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ItemCardapioRequestDTO dto)
        {
            var item = await _context.ItemCardapios.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            item.Nome = dto.Nome;
            item.Descricao = dto.Descricao;
            item.Preco = dto.Preco;
            item.Periodo = dto.Periodo;
            item.ImagemUrl = dto.ImagemUrl;

            _context.ItemCardapios.Update(item);
            await _context.SaveChangesAsync();

            var response = new ItemCardapioResponseDTO(
                item.Id,
                item.Nome,
                item.Descricao,
                item.Preco,
                item.Periodo,
                item.ImagemUrl,
                item.Ativo // ← CORREÇÃO AQUI
            );

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Deletar(int id)
        {
            var item = await _context.ItemCardapios.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            _context.ItemCardapios.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item deletado com sucesso.");
        }
    }
}