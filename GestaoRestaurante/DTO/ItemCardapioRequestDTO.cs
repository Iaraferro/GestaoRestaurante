using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record ItemCardapioRequestDTO(
       string Nome,
       string Descricao,
       decimal Preco,
       PeriodoCardapio Periodo,
       string? ImagemUrl
        );
    
}
