namespace GestaoRestaurante.Models
{
    public class Reserva : EntidadeBase
    {
        public DateTime DataHora { get; set; }

        //Relacionemento com Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string CodigoConfirmacao { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
        //Relacionamneto com Mesa
        public int MesaId { get; set; }
        public Mesa Mesa { get; set; } = null!;
    }
}
