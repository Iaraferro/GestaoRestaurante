namespace GestaoRestaurante.Models
{
    public class SugestaoDoChefe: EntidadeBase
    {
        public DateTime Data { get; set; } // qual dia é a sugestão
        public PeriodoCardapio Periodo { get; set; }

        //FK para o itm escolhido
        public int ItemCardapioId {  get; set; }
        public ItemCardapio ItemCardapio { get; set; } = null!;

        public decimal PercentualDesconto { get; set; } = 0.20m; // 20% fixo por ora
    }
}
