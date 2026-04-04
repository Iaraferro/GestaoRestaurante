using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record ItemCardapioResponseDTO(
        int Id,
        string Nome,
        string Descricao,
        decimal Preco,
        PeriodoCardapio Periodo,
        string? ImagemUrl,
        bool Ativo        // ← NOVO
    );
}
