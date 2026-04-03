using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using GestaoRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public PedidoController(RestauranteContext context)
        {
            _context = context;
        }
       

        [HttpGet]
        [EndpointSummary("Lista todos os pedidos")]
        public async Task<IActionResult> ListarTodos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Itens)
                .Include(p => p.Atendimento)
                .Select(p => new PedidoResponseDTO(
                    p.Id,
                    p.DataHoraPedido,
                    p.Status,
                    p.Periodo,
                    p.TotalItens,
                    p.TaxaAtendimento,
                    p.TotalFinal
                ))
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca um pedido pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Itens)
                .Include(p => p.Atendimento)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            var response = new PedidoResponseDTO(
                pedido.Id,
                pedido.DataHoraPedido,
                pedido.Status,
                pedido.Periodo,
                pedido.TotalItens,
                pedido.TaxaAtendimento,
                pedido.TotalFinal
            );

            return Ok(response);
        }

        [HttpPost]
        [EndpointSummary("Cria um novo pedido")]
        public async Task<IActionResult> CriarPedido(PedidoRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var tipoAtendimento = dto.TipoAtendimento.ToString();
            var tipoNome = dto.TipoAtendimento switch
            {
                TipoAtendimento.Presencial => "Presencial",
                TipoAtendimento.DeliveryProprio => "DeliveryProprio",
                TipoAtendimento.DeliveryApp => "DeliveryApp",
                _ => throw new Exception("Tipo inválido")
            };

            var atendimento = await _context.Atendimentos
                .FirstOrDefaultAsync(a => EF.Property<string>(a, "TipoAtendimento") == tipoNome);
            if (atendimento == null)
                return NotFound("Tipo de atendimento não encontrado.");

            var pedido = new Pedido
            {
                UsuarioId = dto.UsuarioId,
                Periodo = dto.Periodo,
                AtendimentoId = atendimento.Id,
                DataHoraPedido = DateTime.Now,
                Status = StatusPedido.Recebido,
                Itens = new List<ItemPedido>()
            };

            decimal totalItens = 0;

            foreach (var itemDto in dto.Itens)
            {
                var itemCardapio = await _context.ItemCardapios.FindAsync(itemDto.ItemCardapioId);
                if (itemCardapio == null)
                    return NotFound($"Item {itemDto.ItemCardapioId} não encontrado.");

                if (itemCardapio.Periodo != dto.Periodo)
                    return BadRequest($"Item {itemCardapio.Nome} não pertence ao período do pedido.");

                var sugestao = await _context.SugestoesDoChefe
                    .FirstOrDefaultAsync(s => s.ItemCardapioId == itemDto.ItemCardapioId
                        && s.Data.Date == DateTime.Today
                        && s.Periodo == dto.Periodo);

                var desconto = sugestao != null ? itemCardapio.Preco * 0.20m : 0;
                var precoMomento = itemCardapio.Preco;
                var totalItem = (precoMomento - desconto) * itemDto.Quantidade;

                pedido.Itens.Add(new ItemPedido
                {
                    ItemCardapioId = itemDto.ItemCardapioId,
                    Quantidade = itemDto.Quantidade,
                    PrecoMomento = precoMomento,
                    DescontoAplicado = desconto,
                    TotalItem = totalItem,
                    Observacao = itemDto.Observacao
                });

                totalItens += totalItem;
            }

            pedido.TotalItens = totalItens;
            pedido.TaxaAtendimento = atendimento.CalcularTaxa(totalItens);
            pedido.TotalFinal = totalItens + pedido.TaxaAtendimento;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var response = new PedidoResponseDTO(
                pedido.Id,
                pedido.DataHoraPedido,
                pedido.Status,
                pedido.Periodo,
                pedido.TotalItens,
                pedido.TaxaAtendimento,
                pedido.TotalFinal
            );

            return CreatedAtAction(nameof(BuscarPorId), new { id = pedido.Id }, response);
        }

        [HttpPut("{id}/status")]
        [EndpointSummary("Atualiza o status de um pedido")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] StatusPedido novoStatus)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            pedido.Status = novoStatus;
            await _context.SaveChangesAsync();

            return Ok("Status atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Cancela um pedido")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            pedido.Status = StatusPedido.Cancelado;
            await _context.SaveChangesAsync();

            return Ok("Pedido cancelado com sucesso.");
        }
    }
}
