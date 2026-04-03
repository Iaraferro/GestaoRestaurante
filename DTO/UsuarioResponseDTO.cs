using GestaoRestaurante.Models;

namespace GestaoRestaurante.DTO
{
    public record UsuarioResponseDTO(
        int Id,
        string UserName,
        PerfilUsuario Perfil
        );
    
}
