namespace GestaoRestaurante.DTO
{
    public record IngredienteResponseDTO(
        int Id,
        string Nome,
        string? Descricao,
        decimal EstoqueAtual,
        decimal EstoqueMinimo,
        string UnidadeMedida,
        string? DescricaoAlergeno,
        bool Alergeno
        );
    
}
