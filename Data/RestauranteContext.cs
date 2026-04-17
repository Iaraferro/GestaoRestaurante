using GestaoRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoRestaurante.Data
{
    public class RestauranteContext : DbContext
    {
        public RestauranteContext(DbContextOptions<RestauranteContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }    
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<ItemCardapio> ItemCardapios { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<ItemIngrediente> ItemIngredientes { get; set; }
        public DbSet<SugestaoDoChefe> SugestoesDoChefe { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Herança de Atendimento
            modelBuilder.Entity<Atendimento>()
                .HasDiscriminator<string>("TipoAtendimento")
                .HasValue<AtendimentoPresencial>("Presencial")
                .HasValue<AtendimentoDeliveryProprio>("DeliveryProprio")
                .HasValue<AtendimentoDeliveryApp>("DeliveryApp");

            // Índice único: 1 sugestão por período por dia
            modelBuilder.Entity<SugestaoDoChefe>()
                .HasIndex(s => new { s.Data, s.Periodo })
                .IsUnique();
            modelBuilder.Entity<Usuario>()
                .Property(s => s.Password)
                .HasMaxLength(225)
                .IsRequired();
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(112)
                .IsRequired();
            // Precisão de decimais
            modelBuilder.Entity<ItemCardapio>()
                .Property(i => i.Preco)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TotalFinal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.PrecoMomento)
                .HasPrecision(10, 2);

            modelBuilder.Entity<AtendimentoDeliveryProprio>()
    .Property(a => a.TaxaFixa)
    .HasPrecision(10, 2);

            modelBuilder.Entity<Ingrediente>()
                .Property(i => i.EstoqueAtual)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Ingrediente>()
                .Property(i => i.EstoqueMinimo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ItemIngrediente>()
                .Property(i => i.Quantidade)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.DescontoAplicado)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.TotalItem)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TaxaAtendimento)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TotalItens)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SugestaoDoChefe>()
                .Property(s => s.PercentualDesconto)
                .HasPrecision(10, 2);
                
            // ✅ ADICIONE ESTA LINHA NO FINAL:
            SeedIngredientes.Seed(modelBuilder);
        }
    }
}