using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record PedidoRequestDTO(
    int UsuarioId,
    PeriodoCardapio Periodo,
    TipoAtendimento TipoAtendimento,
    MetodoPagamento MetodoPagamento,
    List<ItemPedidoRequestDTO> Itens
    );
    
}
