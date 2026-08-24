# GestaoRestaurante API

API REST para gerenciamento de restaurante, desenvolvida em ASP.NET Core 10 com Entity Framework Core, SQL Server e BCrypt para hash de senhas. O sistema gerencia cardápio, pedidos, reservas, mesas, ingredientes, usuários e relatórios.

---

## Funcionalidades

- Usuários: cadastro, login com hash de senha (BCrypt) e perfis (Usuário/Administrador).
- Cardápio: CRUD de itens com nome, descrição, preço, período (Almoço/Jantar) e imagem. Limite de 20 itens por período.
- Pedidos: criação com múltiplos itens, cálculo automático de desconto de 20% para sugestão do chefe, taxas por tipo de atendimento (Presencial, Delivery Próprio, Delivery App) e total final.
- Reservas: agendamento de mesas com código de confirmação único de 8 dígitos, validação de horário (11h às 14h) e antecedência mínima de 1 dia.
- Ingredientes: cadastro com estoque, unidade de medida, alerta de alergênicos e vínculo com itens do cardápio.
- Sugestões do Chefe: define o prato do dia com 20% de desconto para cada período.
- Relatórios: faturamento por tipo de atendimento e lista dos itens mais vendidos (com e sem desconto).
- Seed de dados: usuário admin, mesas, 40 itens de cardápio e tipos de atendimento pré-cadastrados.

---

## Tecnologias

- .NET 10
- ASP.NET Core
- Entity Framework Core (Code-First)
- SQL Server
- BCrypt.Net-Next
- Swagger / OpenAPI

---

## Como executar

**Pré-requisitos**

- .NET 10 SDK
- SQL Server (ou SQL Server Express / LocalDB)
- Git

**Clonar o repositório**

```bash
git clone https://github.com/Iaraferro/GestaoRestaurante.git
cd GestaoRestaurante
Configurar a string de conexão

No arquivo appsettings.json, ajuste a conexão:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=RestauranteDB;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
}
Aplicar as migrações

bash
dotnet ef database update
Executar a aplicação

bash
dotnet run
A API estará disponível em:

HTTP: http://localhost:5203

HTTPS: https://localhost:7021

Acessar a documentação Swagger

Abra no navegador: http://localhost:5203/swagger

Dados de teste (Seed)
Ao executar o projeto pela primeira vez, o sistema cria automaticamente:

Usuário administrador: admin / admin123

3 tipos de atendimento: Presencial, Delivery Próprio (taxa fixa R$ 5,00), Delivery App

5 mesas com capacidades de 2 a 8 lugares

40 itens de cardápio (20 para Almoço e 20 para Jantar)

Exemplos de endpoints
Método	Endpoint	Descrição
POST	/api/usuario/cadastrar	Cadastra um novo usuário
POST	/api/usuario/login	Realiza login
GET	/api/cardapio	Lista itens do cardápio (filtro por período)
POST	/api/pedido	Cria um novo pedido
GET	/api/reserva/mesas-disponiveis?data=2026-08-25	Lista mesas disponíveis para uma data
GET	/api/relatorio/faturamento	Faturamento por tipo de atendimento em um período
GET	/api/relatorio/itens-mais-vendidos	Itens mais vendidos

Estrutura do Projeto
text
GestaoRestaurante/
├── Controllers/          # Endpoints da API
│   ├── UsuarioController.cs
│   ├── PedidoController.cs
│   ├── ReservaController.cs
│   ├── ItemCardapioController.cs
│   ├── IngredienteController.cs
│   ├── RelatorioController.cs
│   └── SugestaoDoChefeController.cs
├── Models/               # Entidades do banco de dados
│   ├── Usuario.cs
│   ├── Pedido.cs
│   ├── ItemPedido.cs
│   ├── ItemCardapio.cs
│   ├── Ingrediente.cs
│   ├── Reserva.cs
│   ├── Mesa.cs
│   ├── Atendimento.cs (abstrata)
│   ├── AtendimentoPresencial.cs
│   ├── AtendimentoDeliveryProprio.cs
│   └── AtendimentoDeliveryApp.cs
├── DTOs/                 # Objetos de transferência (Request/Response)
│   ├── UsuarioRequestDTO.cs
│   ├── PedidoRequestDTO.cs
│   ├── ItemCardapioResponseDTO.cs
│   └── ...
├── Data/                 # Contexto do EF Core
│   └── RestauranteContext.cs
├── Program.cs            # Configuração e seed de dados
└── appsettings.json      # Configurações (connection string, etc.)

Melhorias Futuras
□ Implementar autenticação com JWT para proteger endpoints.
□ Separar a lógica de negócio em uma camada de Services.
□ Adicionar testes unitários com xUnit.
□ Adicionar logs com Serilog para monitoramento.
GitHub | LinkedIn
Estudante de Sistemas de Informação – UNITINS
