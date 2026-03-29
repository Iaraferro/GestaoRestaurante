using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record SugestaoDoChefeResponseDTO(
    int Id,
    DateTime Data,
    PeriodoCardapio Periodo,
    int ItemCardapioId
    );
    
}
