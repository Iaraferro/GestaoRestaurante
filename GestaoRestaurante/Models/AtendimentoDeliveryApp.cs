namespace GestaoRestaurante.Models
{
    public class AtendimentoDeliveryApp : Atendimento
    {
        public override decimal CalcularTaxa(decimal valorPedido)
        {
            if (DateTime.Now.Hour < 18)
                return valorPedido * 0.04m;
            else
                return valorPedido * 0.06m;
        }
    }
}
