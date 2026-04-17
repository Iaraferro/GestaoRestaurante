namespace GestaoRestaurante.DTO
{
    public record EnderecoRequestDTO(
        string Rua,
        int Numero,
        string? Complemento,
        string Bairro,
        string Cidade,
        string Estado,
        string Cep,
        int UsuarioId
    );
}
