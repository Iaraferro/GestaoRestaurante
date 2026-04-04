using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record PedidoResponseDTO(
        int Id,
        DateTime DataHoraPedido,
        StatusPedido Status,
        PeriodoCardapio Periodo,
        decimal TotalItens,
        decimal TaxaAtendimento,
        decimal TotalFinal,
        List<ItemPedidoResponseDTO> Itens
    );
}
