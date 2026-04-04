namespace GestaoRestaurante.DTO
{
    public record ItemPedidoResponseDTO(
        int Id,
        int ItemCardapioId,
        string Nome,
        string? ImagemUrl,
        int Quantidade,
        decimal PrecoMomento,
        decimal DescontoAplicado,
        decimal TotalItem,
        string? Observacao
    );
}