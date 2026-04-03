using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record SugestaoDoChefeRequestDTO(
    DateTime Data,
    PeriodoCardapio Periodo,
    int ItemCardapioId
    );
    
}
