namespace GestaoRestaurante.DTO
{
    public record ItemPedidoRequestDTO(
    int ItemCardapioId,
    int Quantidade,
    string? Observacao
       );
    
}
