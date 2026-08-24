# GestaoRestaurante
markdown
# 🍽️ GestaoRestaurante API

API REST para gerenciamento completo de restaurante, desenvolvida em **ASP.NET Core 10** com **Entity Framework Core**, **SQL Server** e **BCrypt** para segurança de senhas. O sistema contempla cardápio, pedidos, reservas, mesas, ingredientes, usuários, relatórios e sugestões do chefe.

---

## ✨ Funcionalidades Principais

- **👤 Usuários**: Cadastro, login com hash de senha (BCrypt), perfis (Usuário/Administrador).
- **📋 Cardápio**: CRUD de itens com nome, descrição, preço, período (Almoço/Jantar) e imagem. Limite de **20 itens por período**.
- **🧾 Pedidos**: Criação de pedidos com múltiplos itens, cálculo automático de:
  - Desconto de **20%** se o item for a **Sugestão do Chefe** do dia.
  - Taxas de atendimento (Presencial = 0%, Delivery Próprio = taxa fixa, Delivery App = 4% ou 6%).
  - Total final com itens + taxa.
- **🍽️ Reservas**: Agendamento de mesas com código de confirmação único (8 dígitos). Validação de horário (11h-14h) e antecedência mínima de 1 dia.
- **🥩 Ingredientes**: Cadastro com estoque, unidade de medida, alerta de alergênicos e vínculo com itens do cardápio.
- **⭐ Sugestões do Chefe**: Define o prato do dia (com 20% de desconto) para cada período.
- **📊 Relatórios**: Faturamento por tipo de atendimento e lista dos itens mais vendidos (com e sem desconto).
- **📦 Seed de Dados**: Já vem com usuário admin, mesas, 40 itens de cardápio e tipos de atendimento pré-cadastrados.

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Descrição |
| :--- | :--- |
| **.NET 10** | Framework principal |
| **ASP.NET Core** | Criação de APIs REST |
| **Entity Framework Core** | ORM para acesso a dados (Code-First) |
| **SQL Server** | Banco de dados relacional |
| **BCrypt.Net-Next** | Hash de senhas |
| **Swagger / OpenAPI** | Documentação interativa da API |
| **CORS** | Liberação para consumo de front-ends |

---

## 🚀 Como Executar o Projeto

### 1. Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (ou SQL Server Express / LocalDB)
- [Git](https://git-scm.com/)

### 2. Clonar o repositório
```bash
git clone https://github.com/Iaraferro/GestaoRestaurante.git
cd GestaoRestaurante
3. Configurar a string de conexão
No arquivo appsettings.json, ajuste a conexão com seu SQL Server:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=RestauranteDB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
}
4. Aplicar as migrações (criar o banco de dados)
bash
dotnet ef database update
5. Executar a aplicação
bash
dotnet run
A API estará disponível em:

http://localhost:5203 (HTTP)

https://localhost:7021 (HTTPS)

6. Acessar a documentação Swagger
Abra no navegador: http://localhost:5203/swagger

🧪 Dados de Teste (Seed)
Ao executar o projeto pela primeira vez, o sistema cria automaticamente:

Usuário administrador: admin / admin123

3 tipos de atendimento: Presencial, Delivery Próprio (taxa R$5,00), Delivery App

5 mesas com capacidades variadas (2 a 8 lugares)

40 itens de cardápio (20 para Almoço, 20 para Jantar) com preços e descrições

📌 Exemplos de Endpoints
Método	Endpoint	Descrição
POST	/api/usuario/cadastrar	Cadastra um novo usuário
POST	/api/usuario/login	Realiza login (retorna dados do usuário)
GET	/api/cardapio	Lista todos os itens do cardápio (filtra por período com ?periodo=Almoco)
POST	/api/pedido	Cria um novo pedido (com itens, descontos e taxas)
GET	/api/reserva/mesas-disponiveis?data=2026-08-25	Lista mesas disponíveis para uma data
GET	/api/relatorio/faturamento?dataInicio=2026-08-01&dataFim=2026-08-31	Faturamento por tipo de atendimento
GET	/api/relatorio/itens-mais-vendidos	Itens mais vendidos (com filtro de data opcional)
📂 Estrutura do Projeto
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
🔮 Próximos Passos (Melhorias Futuras)
□ Implementar autenticação com JWT para proteger endpoints.
□ Separar a lógica de negócio em uma camada de Services.
□ Adicionar testes unitários com xUnit.
□ Criar um front-end em Angular para consumir a API.
□ Adicionar logs com Serilog para monitoramento.
Iara Martins Ferro
GitHub | LinkedIn
Estudante de Sistemas de Informação – UNITINS
