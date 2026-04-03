namespace GestaoRestaurante.DTO
{
    public record ReservaResponseDTO(
        int Id,
        DateTime DataHora,
        int MesaId,
        string CodigoConfirmacao
     );
    
}
