using GestaoRestaurante.Data;
using GestaoRestaurante.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemPedidoController: ControllerBase
    {
        private readonly RestauranteContext _context;

        public ItemPedidoController(RestauranteContext context)
        {
            _context = context;
        }

        [HttpGet("pedido/{pedidoId}")]
        [EndpointSummary("Lista os itens de um pedido")]
        public async Task<IActionResult> ListarPorPedido(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            var itens = await _context.ItensPedido
                .Include(ip => ip.ItemCardapio) // ← NOVO: Traz os dados do prato
                .Where(ip => ip.PedidoId == pedidoId)
                .Select(ip => new ItemPedidoResponseDTO(
                    ip.Id,
                    ip.ItemCardapioId,           // ← NOVO
                    ip.ItemCardapio.Nome,        // ← NOVO
                    ip.ItemCardapio.ImagemUrl,   // ← NOVO
                    ip.Quantidade,
                    ip.PrecoMomento,
                    ip.DescontoAplicado,
                    ip.TotalItem,
                    ip.Observacao
                ))
                .ToListAsync();

            return Ok(itens);
        }

        [HttpGet("{id}")]
        [EndpointSummary("Busca um item de pedido pelo ID")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var item = await _context.ItensPedido
                .Include(ip => ip.ItemCardapio) // ← NOVO: Traz os dados do prato
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (item == null)
                return NotFound("Item do pedido não encontrado.");

            var response = new ItemPedidoResponseDTO(
                item.Id,
                item.ItemCardapioId,           // ← NOVO
                item.ItemCardapio.Nome,        // ← NOVO
                item.ItemCardapio.ImagemUrl,   // ← NOVO
                item.Quantidade,
                item.PrecoMomento,
                item.DescontoAplicado,
                item.TotalItem,
                item.Observacao
            );

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Remove um item de um pedido")]
        public async Task<IActionResult> Deletar(int id)
        {
            var item = await _context.ItensPedido.FindAsync(id);

            if (item == null)
                return NotFound("Item do pedido não encontrado.");

            _context.ItensPedido.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removido do pedido.");
        }
    }
}