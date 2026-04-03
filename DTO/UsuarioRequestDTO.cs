namespace GestaoRestaurante.DTO
{
    public record UsuarioRequestDTO(
        string UserName,
        string PasswordHasher,
        string Email
        );
    
}
