namespace GestaoRestaurante.DTO
{
    public record EnderecoResponseDTO(
        int Id,
        string Rua,
        int Numero,
        string Bairro,
        string Cidade,
        string Estado
        );
    
}
