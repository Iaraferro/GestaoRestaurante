using GestaoRestaurante.Models;
using System.Data;

namespace GestaoRestaurante.DTO
{
    public record ReservaRequestDTO(
        int MesaId,
        int UsuarioId,
        DateTime DataHora
        );
    
}
