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
    public class SugestaoDoChefController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public SugestaoDoChefController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Lista todas as sugestões do chefe")]
        public async Task<IActionResult> ListarTodas()
        {
            var sugestoes = await _context.SugestoesDoChefe
                .Include(s => s.ItemCardapio)
                .Select(s => new SugestaoDoChefeResponseDTO(
                    s.Id,
                    s.Data,
                    s.Periodo,
                    s.ItemCardapioId
                ))
                .ToListAsync();

            return Ok(sugestoes);
        }

        [HttpGet("hoje")]
        [EndpointSummary("Lista as sugestões do chefe para hoje")]
        public async Task<IActionResult> ListarHoje()
        {
            var sugestoes = await _context.SugestoesDoChefe
                .Include(s => s.ItemCardapio)
                .Where(s => s.Data.Date == DateTime.Today)
                .Select(s => new SugestaoDoChefeResponseDTO(
                    s.Id,
                    s.Data,
                    s.Periodo,
                    s.ItemCardapioId
                ))
                .ToListAsync();

            if (!sugestoes.Any())
                return NotFound("Nenhuma sugestão cadastrada para hoje.");

            return Ok(sugestoes);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")] 
        [EndpointSummary("Define a sugestão do chefe do dia")]
        public async Task<IActionResult> CriarSugestao(SugestaoDoChefeRequestDTO dto)
        {
            // Valida se o item existe
            var item = await _context.ItemCardapios.FindAsync(dto.ItemCardapioId);
            if (item == null)
                return NotFound("Item do cardápio não encontrado.");

            // Valida se o item pertence ao período informado
            if (item.Periodo != dto.Periodo)
                return BadRequest($"O item '{item.Nome}' não pertence ao período {dto.Periodo}.");

            // Valida se já existe sugestão para esse período nesse dia
            var jaExiste = await _context.SugestoesDoChefe
                .AnyAsync(s => s.Data.Date == dto.Data.Date
                    && s.Periodo == dto.Periodo);

            if (jaExiste)
                return BadRequest($"Já existe uma sugestão do chefe para {dto.Periodo} nessa data.");

            var sugestao = new SugestaoDoChefe
            {
                Data = dto.Data,
                Periodo = dto.Periodo,
                ItemCardapioId = dto.ItemCardapioId,
                PercentualDesconto = 0.20m
            };

            _context.SugestoesDoChefe.Add(sugestao);
            await _context.SaveChangesAsync();

            var response = new SugestaoDoChefeResponseDTO(
                sugestao.Id,
                sugestao.Data,
                sugestao.Periodo,
                sugestao.ItemCardapioId
            );

            return CreatedAtAction(nameof(ListarHoje), response);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove uma sugestão do chefe")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sugestao = await _context.SugestoesDoChefe.FindAsync(id);

            if (sugestao == null)
                return NotFound("Sugestão não encontrada.");

            _context.SugestoesDoChefe.Remove(sugestao);
            await _context.SaveChangesAsync();

            return Ok("Sugestão removida com sucesso.");
        }
    }
}

