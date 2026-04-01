using GestaoRestaurante.Data;
using GestaoRestaurante.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RestauranteContext>();

    if (!context.Atendimentos.Any())
    {
        context.Atendimentos.AddRange(
            new AtendimentoPresencial
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new AtendimentoDeliveryProprio
            {
                TaxaFixa = 5.00m,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new AtendimentoDeliveryApp
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        );
        context.SaveChanges();
    }

    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new Usuario
        {
            UserName = "admin",
            Email = "admin@restaurante.com",
            Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Perfil = PerfilUsuario.Administrador,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        });
        context.SaveChanges();
    }
    if (!context.Mesas.Any())
    {
        context.Mesas.AddRange(
            new Mesa { Numero = 1, Capacidade = 2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Mesa { Numero = 2, Capacidade = 4, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Mesa { Numero = 3, Capacidade = 4, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Mesa { Numero = 4, Capacidade = 6, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Mesa { Numero = 5, Capacidade = 8, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        );
        context.SaveChanges();
    }
    if (!context.ItemCardapios.Any())
    {
        context.ItemCardapios.AddRange(
            // Almoço
            new ItemCardapio { Nome = "Caldo de Cana Consommé", Descricao = "Consommé de caldo de cana com capim-limão, gengibre e espuma de coco fresco", Preco = 38.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Pirarucu ao Tucupi Negro", Descricao = "Filé de pirarucu selado com crosta de castanha, tucupi negro reduzido e jambu fresco", Preco = 89.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Risoto de Queijo Canastra", Descricao = "Risoto mantecato com queijo Canastra curado 6 meses, lascas de carne seca e crocante de mandioca", Preco = 78.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Galinhada do Cerrado", Descricao = "Frango caipira confitado com açafrão da terra, arroz de pequi e farofa de baru torrado", Preco = 82.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Moqueca Contemporânea de Caju", Descricao = "Castanha e pedúnculo de caju ao molho de dendê clarificado com leite de coco e arroz negro", Preco = 74.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Baião de Dois Alma Nativa", Descricao = "Arroz com feijão verde do sertão, queijo coalho grelhado na brasa, carne seca desfiada e crocante de tapioca", Preco = 76.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Cordeiro do Pantanal", Descricao = "Carré de cordeiro pantaneiro assado lentamente com ervas nativas, purê de mandioca cremoso e redução de jambu", Preco = 95.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Cuscuz Paulista de Lagostim", Descricao = "Cuscuz de fubá artesanal com lagostim de rio, palmito pupunha e molho bisque de carapaça", Preco = 92.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Frango Caipira à Moda Mineira", Descricao = "Frango caipira cozido lentamente com quiabo, ora-pro-nóbis e angu de fubá trançado", Preco = 72.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Tapioca Recheada Gourmet", Descricao = "Tapioca de goma fresca recheada com camarão ao azeite de dendê, catupiry artesanal e chips de macaxeira", Preco = 68.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Costela Bovina ao Pequi", Descricao = "Costela bovina assada 12 horas com molho de pequi do cerrado, farofa de castanha do Pará e couve mineira", Preco = 98.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Sopa de Mandioca com Camarão", Descricao = "Velouté de mandioca amarela com camarão rosa salteado, azeite de dendê e crocante de tapioca", Preco = 62.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Filé de Tambaqui Grelhado", Descricao = "Tambaqui da Amazônia grelhado na folha de bananeira com vinagrete de cupuaçu e arroz de jambu", Preco = 86.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Nhoque de Macaxeira", Descricao = "Nhoque artesanal de macaxeira ao molho de queijo Canastra com cogumelos nativos e azeite trufado", Preco = 70.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Picanha Maturada com Farofa", Descricao = "Picanha maturada 28 dias grelhada na brasa com farofa de dendê, vinagrete de pimenta biquinho e mandioca frita", Preco = 105.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Salada de Flores Nativas", Descricao = "Mix de folhas do cerrado, flores comestíveis, castanha de baru, vinagrete de maracujá e queijo Minas curado", Preco = 48.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Bobó de Camarão Contemporâneo", Descricao = "Camarão rosa ao molho cremoso de mandioca com dendê clarificado, servido com arroz branco e farofa crocante", Preco = 88.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Joelho de Porco Caipira", Descricao = "Joelho de porco caipira braseado com cerveja artesanal, purê de inhame e chucrute de couve", Preco = 80.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Robalo ao Leite de Castanha", Descricao = "Filé de robalo pochê em leite de castanha do Pará com purê de banana da terra e vinagrete de tucupi", Preco = 90.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Duo de Queijos Brasileiros", Descricao = "Seleção de queijos Canastra, Coalho e Minas Padrão com geleia de goiaba artesanal, mel e castanhas", Preco = 55.00m, Periodo = PeriodoCardapio.Almoco, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },

            // Jantar
            new ItemCardapio { Nome = "Tartar de Atum com Açaí", Descricao = "Tartar de atum fresco com creme de açaí, ovas de peixe, chips de tapioca e azeite de ervas nativas", Preco = 72.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Lagosta ao Molho de Maracujá", Descricao = "Lagosta inteira grelhada na manteiga noisette com molho agridoce de maracujá do cerrado e purê de mandioca", Preco = 185.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Filé Mignon ao Molho de Cachaça", Descricao = "Filé mignon maturado 45 dias flambado na cachaça artesanal com molho de jabuticaba e batata suflê", Preco = 145.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Camarão Imperial da Bahia", Descricao = "Camarão gigante grelhado ao azeite de dendê com risoto de leite de coco e redução de pimenta malagueta", Preco = 128.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Pato Confitado com Tucupi", Descricao = "Pato confitado lentamente servido com molho de tucupi e jambu, purê de macaxeira trufado e farofa d'água", Preco = 138.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Duo de Frutos do Mar", Descricao = "Vieiras seladas e camarão ao bisque de crustáceos com risoto negro de tinta de lula e espuma de ervas", Preco = 155.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Cordeiro ao Vinho do Porto", Descricao = "Carré de cordeiro ao molho de vinho do Porto com purê de inhame com azeite e legumes do cerrado assados", Preco = 162.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Risoto Negro de Lula", Descricao = "Risoto de tinta de lula com anéis de lula grelhados, camarão rosa e espuma de limão siciliano", Preco = 118.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Polvo Braseado com Mandioca", Descricao = "Polvo braseado lentamente com azeite e ervas, servido com chips de mandioca e vinagrete de pimenta biquinho", Preco = 142.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Salmão com Crosta de Castanha", Descricao = "Salmão selado com crosta de castanha do Pará, molho de maracujá e purê de batata doce com açafrão", Preco = 122.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Medalhão de Javali", Descricao = "Medalhão de javali com molho de goiabada e pimenta, purê de mandioca baroa e crocante de baru", Preco = 168.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Bacalhau à Brasileira", Descricao = "Lombo de bacalhau dessalgado com azeite de ervas, farofa de dendê, grão de bico salteado e ovo de codorna", Preco = 135.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Nhoque de Pupunha", Descricao = "Nhoque artesanal de pupunha ao molho de manteiga de garrafa com cogumelos shiitake e queijo Canastra ralado", Preco = 98.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Costela de Wagyu Brasileira", Descricao = "Short rib de Wagyu brasileiro assado 18 horas com molho de açaí e cachaça, purê de mandioca e couve crocante", Preco = 195.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Frango Caipira Recheado", Descricao = "Frango caipira recheado com farofa de castanha e ervas do cerrado, ao molho de pequi e batata baroa assada", Preco = 108.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Vieiras ao Molho de Cupuaçu", Descricao = "Vieiras seladas em manteiga noisette com molho de cupuaçu, espuma de leite de coco e ovas de tapioca", Preco = 148.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Duo de Filés Alma Nativa", Descricao = "Medalhão de filé mignon e filé de pirarucu com dois molhos — jabuticaba e tucupi negro — e purê de mandioca", Preco = 175.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Taça de Frutos do Cerrado", Descricao = "Composição de pequi, baru, cagaita e murici com sorvete de cajá e espuma de mel de abelha nativa", Preco = 68.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Robalo com Farofa de Dendê", Descricao = "Filé de robalo grelhado na folha de bananeira com farofa de dendê, vinagrete de tomate e pimenta cambuci", Preco = 132.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new ItemCardapio { Nome = "Tábua Alma Nativa", Descricao = "Seleção de carnes curadas, queijos brasileiros artesanais, conservas da estação, pães de fermentação natural e geleias", Preco = 88.00m, Periodo = PeriodoCardapio.Jantar, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        );
     
            context.SaveChanges();
    }
}




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();


app.Run();