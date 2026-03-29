namespace GestaoRestaurante.DTO
{
    public record IngredienteRequestDTO(
         string Nome,
         string? Descricao,
         decimal EstoqueAtual,
         decimal EstoqueMinimo,
         string UnidadeMedida,
         string? DescricaoAlergeno,
         bool Alergeno
        );
    
}
