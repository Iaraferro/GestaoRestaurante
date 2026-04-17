namespace GestaoRestaurante.DTO
{
    public record EnderecoResponseDTO(
        int Id,
        string Rua,
        int Numero,
        string? Complemento,
        string Bairro,
        string Cidade,
        string Estado,
        string Cep
    );
}
