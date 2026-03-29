namespace GestaoRestaurante.DTO
{
    public record EnderecoRequestDTO(
        int UsuarioId,
        string Rua,
        int Numero,
        string Bairro,
        string Cidade,
        string Estado
        );
    
}
