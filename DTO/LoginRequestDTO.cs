namespace GestaoRestaurante.DTO
{
    public record LoginRequestDTO(
        string Email,
        string PasswordHasher
    );
}