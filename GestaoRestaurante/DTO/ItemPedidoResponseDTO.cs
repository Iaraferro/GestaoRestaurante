namespace GestaoRestaurante.DTO
{
    public record ItemPedidoResponseDTO(
    int Id,
    int Quantidade,
    decimal PrecoMomento,
    decimal DescontoAplicado,
    decimal TotalItem,
    string? Observacao
    );
    
}
