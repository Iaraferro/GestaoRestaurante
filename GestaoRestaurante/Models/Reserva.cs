namespace GestaoRestaurante.Models
{
    public class Reserva : EntidadeBase
    {
        public DateTime DataHora { get; set; }

        //Relacionemento com Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        //Relacionamneto com Mesa
        public int MesaId { get; set; }
        public Mesa Mesa { get; set; }
    }
}
