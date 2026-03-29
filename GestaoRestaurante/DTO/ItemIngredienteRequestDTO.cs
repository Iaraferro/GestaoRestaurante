namespace GestaoRestaurante.DTO
{
    public record ItemIngredienteRequestDTO(
        int ItemCardapioId,
        int IngredienteId,
        decimal Quantidade,
        string Unidade
        );
    
}
