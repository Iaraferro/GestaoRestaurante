using GestaoRestaurante.Data;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] 
    public class RelatorioController: ControllerBase
    {
        private readonly RestauranteContext _context;

        public RelatorioController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet("faturamento")]
        [EndpointSummary("Faturamento total por tipo de atendimento em um período")]
        public async Task<IActionResult> FaturamentoPorTipoAtendimento(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Atendimento)
                .Where(p => p.DataHoraPedido.Date >= dataInicio.Date
                    && p.DataHoraPedido.Date <= dataFim.Date
                    && p.Status != StatusPedido.Cancelado)
                .ToListAsync();

            var relatorio = pedidos
                .GroupBy(p => p.Atendimento.GetType().Name)
                .Select(g => new
                {
                    TipoAtendimento = g.Key switch
                    {
                        "AtendimentoPresencial" => "Salão (Presencial)",
                        "AtendimentoDeliveryProprio" => "Delivery Próprio",
                        "AtendimentoDeliveryApp" => "iFood / Apps",
                        _ => g.Key
                    },
                    TotalPedidos = g.Count(),
                    Faturamento = g.Sum(p => p.TotalFinal)
                })
                .ToList();

            return Ok(relatorio);
        }

        [HttpGet("itens-mais-vendidos")]
        [EndpointSummary("Lista os itens mais vendidos com e sem desconto")]
        public async Task<IActionResult> ItensMaisVendidos([FromQuery] DateTime? dataInicio = null,
    [FromQuery] DateTime? dataFim = null)
        {
            var query = _context.ItensPedido
            .Include(ip => ip.ItemCardapio)
            .Include(ip => ip.Pedido)
            .Where(ip => ip.Pedido.Status != StatusPedido.Cancelado)
            .AsQueryable();

            if (dataInicio.HasValue)
                query = query.Where(ip => ip.Pedido.DataHoraPedido >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(ip => ip.Pedido.DataHoraPedido <= dataFim.Value);
            var itens = await _context.ItensPedido
                .Include(ip => ip.ItemCardapio)
                .Include(ip => ip.Pedido)
                .Where(ip => ip.Pedido.Status != StatusPedido.Cancelado)
                .GroupBy(ip => new
                {
                    ip.ItemCardapioId,
                    ip.ItemCardapio.Nome,
                    ip.ItemCardapio.Periodo
                })
                .Select(g => new
                {
                    g.Key.Nome,
                    g.Key.Periodo,
                    TotalVendido = g.Sum(ip => ip.Quantidade),
                    VendidoComDesconto = g.Count(ip => ip.DescontoAplicado > 0),
                    VendidoSemDesconto = g.Count(ip => ip.DescontoAplicado == 0)
                })
                .OrderByDescending(i => i.TotalVendido)
                .ToListAsync();


            return Ok(itens);
        }
    }
}
