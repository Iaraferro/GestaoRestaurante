using Microsoft.EntityFrameworkCore;
using GestaoRestaurante.Models;
 
namespace GestaoRestaurante.Data
{
    public static class SeedIngredientes
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var ingredientes = new List<Ingrediente>
            {
                // ── ALMOÇO (ID 1-30) ──
                new() { Id = 1,  Nome = "Caldo de Cana", UnidadeMedida = "litro", EstoqueAtual = 50, EstoqueMinimo = 10, Alergeno = false },
                new() { Id = 2,  Nome = "Capim-limão", UnidadeMedida = "maço", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 3,  Nome = "Gengibre", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 4,  Nome = "Coco Fresco", UnidadeMedida = "unidade", EstoqueAtual = 40, EstoqueMinimo = 10, Alergeno = false },
                new() { Id = 5,  Nome = "Filé de Pirarucu", UnidadeMedida = "kg", EstoqueAtual = 25, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 6,  Nome = "Tucupi Negro", UnidadeMedida = "litro", EstoqueAtual = 12, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 7,  Nome = "Jambu", UnidadeMedida = "maço", EstoqueAtual = 18, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 8,  Nome = "Queijo Canastra", UnidadeMedida = "kg", EstoqueAtual = 30, EstoqueMinimo = 8, Alergeno = true, DescricaoAlergeno = "Contém lactose" },
                new() { Id = 9,  Nome = "Carne Seca", UnidadeMedida = "kg", EstoqueAtual = 22, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 10, Nome = "Mandioca (Macaxeira)", UnidadeMedida = "kg", EstoqueAtual = 60, EstoqueMinimo = 15, Alergeno = false },
                new() { Id = 11, Nome = "Frango Caipira", UnidadeMedida = "kg", EstoqueAtual = 35, EstoqueMinimo = 10, Alergeno = false },
                new() { Id = 12, Nome = "Pequi", UnidadeMedida = "kg", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 13, Nome = "Castanha de Baru", UnidadeMedida = "kg", EstoqueAtual = 18, EstoqueMinimo = 4, Alergeno = true, DescricaoAlergeno = "Oleaginosa" },
                new() { Id = 14, Nome = "Caju", UnidadeMedida = "kg", EstoqueAtual = 25, EstoqueMinimo = 8, Alergeno = false },
                new() { Id = 15, Nome = "Azeite de Dendê", UnidadeMedida = "litro", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 16, Nome = "Feijão Verde", UnidadeMedida = "kg", EstoqueAtual = 40, EstoqueMinimo = 10, Alergeno = false },
                new() { Id = 17, Nome = "Queijo Coalho", UnidadeMedida = "kg", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = true, DescricaoAlergeno = "Contém lactose" },
                new() { Id = 18, Nome = "Cordeiro Pantaneiro", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 19, Nome = "Lagostim de Rio", UnidadeMedida = "kg", EstoqueAtual = 10, EstoqueMinimo = 3, Alergeno = true, DescricaoAlergeno = "Crustáceo" },
                new() { Id = 20, Nome = "Fubá Artesanal / Milho", UnidadeMedida = "kg", EstoqueAtual = 50, EstoqueMinimo = 12, Alergeno = false },
                new() { Id = 21, Nome = "Palmito Pupunha", UnidadeMedida = "kg", EstoqueAtual = 18, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 22, Nome = "Quiabo", UnidadeMedida = "kg", EstoqueAtual = 25, EstoqueMinimo = 8, Alergeno = false },
                new() { Id = 23, Nome = "Ora-pro-nóbis", UnidadeMedida = "maço", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 24, Nome = "Camarão Rosa", UnidadeMedida = "kg", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = true, DescricaoAlergeno = "Crustáceo" },
                new() { Id = 25, Nome = "Costela Bovina", UnidadeMedida = "kg", EstoqueAtual = 30, EstoqueMinimo = 8, Alergeno = false },
                new() { Id = 26, Nome = "Tambaqui da Amazônia", UnidadeMedida = "kg", EstoqueAtual = 18, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 27, Nome = "Picanha Maturada", UnidadeMedida = "kg", EstoqueAtual = 25, EstoqueMinimo = 6, Alergeno = false },
                new() { Id = 28, Nome = "Maracujá do Cerrado", UnidadeMedida = "kg", EstoqueAtual = 22, EstoqueMinimo = 6, Alergeno = false },
                new() { Id = 29, Nome = "Joelho de Porco", UnidadeMedida = "kg", EstoqueAtual = 12, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 30, Nome = "Filé de Robalo", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
 
                // ── JANTAR (ID 31-60) ──
                new() { Id = 31, Nome = "Atum Fresco", UnidadeMedida = "kg", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 32, Nome = "Açaí", UnidadeMedida = "kg", EstoqueAtual = 30, EstoqueMinimo = 8, Alergeno = false },
                new() { Id = 33, Nome = "Tapioca (Goma)", UnidadeMedida = "kg", EstoqueAtual = 25, EstoqueMinimo = 6, Alergeno = false },
                new() { Id = 34, Nome = "Lagosta", UnidadeMedida = "kg", EstoqueAtual = 12, EstoqueMinimo = 3, Alergeno = true, DescricaoAlergeno = "Crustáceo" },
                new() { Id = 35, Nome = "Filé Mignon", UnidadeMedida = "kg", EstoqueAtual = 40, EstoqueMinimo = 10, Alergeno = false },
                new() { Id = 36, Nome = "Cachaça Artesanal", UnidadeMedida = "litro", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 37, Nome = "Jabuticaba", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 38, Nome = "Camarão Gigante", UnidadeMedida = "kg", EstoqueAtual = 18, EstoqueMinimo = 5, Alergeno = true, DescricaoAlergeno = "Crustáceo" },
                new() { Id = 39, Nome = "Leite de Coco", UnidadeMedida = "litro", EstoqueAtual = 35, EstoqueMinimo = 10, Alergeno = false },
                new() { Id = 40, Nome = "Tucupi", UnidadeMedida = "litro", EstoqueAtual = 18, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 41, Nome = "Pato", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 42, Nome = "Vieiras", UnidadeMedida = "kg", EstoqueAtual = 10, EstoqueMinimo = 3, Alergeno = true, DescricaoAlergeno = "Molusco" },
                new() { Id = 43, Nome = "Tinta de Lula", UnidadeMedida = "litro", EstoqueAtual = 8, EstoqueMinimo = 2, Alergeno = true, DescricaoAlergeno = "Molusco" },
                new() { Id = 44, Nome = "Lula (Anéis)", UnidadeMedida = "kg", EstoqueAtual = 12, EstoqueMinimo = 3, Alergeno = true, DescricaoAlergeno = "Molusco" },
                new() { Id = 45, Nome = "Vinho do Porto", UnidadeMedida = "litro", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 46, Nome = "Inhame", UnidadeMedida = "kg", EstoqueAtual = 30, EstoqueMinimo = 8, Alergeno = false },
                new() { Id = 47, Nome = "Polvo", UnidadeMedida = "kg", EstoqueAtual = 10, EstoqueMinimo = 3, Alergeno = true, DescricaoAlergeno = "Molusco" },
                new() { Id = 48, Nome = "Salmão", UnidadeMedida = "kg", EstoqueAtual = 25, EstoqueMinimo = 6, Alergeno = false },
                new() { Id = 49, Nome = "Castanha do Pará", UnidadeMedida = "kg", EstoqueAtual = 20, EstoqueMinimo = 5, Alergeno = true, DescricaoAlergeno = "Oleaginosa" },
                new() { Id = 50, Nome = "Batata Doce", UnidadeMedida = "kg", EstoqueAtual = 45, EstoqueMinimo = 12, Alergeno = false },
                new() { Id = 51, Nome = "Carne de Javali", UnidadeMedida = "kg", EstoqueAtual = 10, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 52, Nome = "Goiabada", UnidadeMedida = "kg", EstoqueAtual = 12, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 53, Nome = "Lombo de Bacalhau", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = false },
                new() { Id = 54, Nome = "Grão de Bico", UnidadeMedida = "kg", EstoqueAtual = 30, EstoqueMinimo = 8, Alergeno = false },
                new() { Id = 55, Nome = "Wagyu Brasileiro", UnidadeMedida = "kg", EstoqueAtual = 8, EstoqueMinimo = 2, Alergeno = false },
                new() { Id = 56, Nome = "Cogumelo Shiitake", UnidadeMedida = "kg", EstoqueAtual = 12, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 57, Nome = "Cupuaçu", UnidadeMedida = "kg", EstoqueAtual = 18, EstoqueMinimo = 5, Alergeno = false },
                new() { Id = 58, Nome = "Cagaita / Murici", UnidadeMedida = "kg", EstoqueAtual = 10, EstoqueMinimo = 3, Alergeno = false },
                new() { Id = 59, Nome = "Queijos Curados", UnidadeMedida = "kg", EstoqueAtual = 15, EstoqueMinimo = 4, Alergeno = true, DescricaoAlergeno = "Contém lactose" },
                new() { Id = 60, Nome = "Manteiga de Garrafa", UnidadeMedida = "kg", EstoqueAtual = 10, EstoqueMinimo = 3, Alergeno = true, DescricaoAlergeno = "Contém lactose" }
            };
 
            modelBuilder.Entity<Ingrediente>().HasData(ingredientes);
        }
    }
}