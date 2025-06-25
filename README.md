## Teste Técnico FSBR - Sistema de Reservas

Teste técnico de construção de uma API .NET e uma aplicação ASP.NET MVC para consumir a API.

### 🚀 Como executar o projeto

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou VS Code (opcional)

### 📥 Configuração inicial

1. Extraia o projeto zipado, ou clone o repositório:
```bash
   git clone https://github.com/vinicius-masson/fsbr-coworking.git
```
3. Abra o arquivo `src/Coworking.API/appsettings.json` e atualize a connection string:
   ```json
   "ConnectionStrings": {
     "CoworkingConnection": "Server=seu_servidor;Database=Coworking;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=true;"
   }
   ```
4. Coloque o e-mail que deseja receber as confirmações de reserva no campo "DestinationEmail"

### 🛠️ Configurar o banco de dados

1. Execute as migrations:

```bash
cd src/Coworking.Infra
dotnet ef database update
```

2. Execute os scripts SQL que estão na pasta "scripts", na raiz do projeto


### ▶️ Executar a aplicação

1. Primeiro inicie a API

```bash
cd src/Coworking.API
dotnet run --launch-profile "https"
```
Acesse a documentação: https://localhost:7123/swagger

2. Depois inicie o projeto MVC

```bash
cd src/Coworking.Web
dotnet run
```

### 🧪 Dados para teste

O script de inserts já cria:

2 usuários

4 salas disponíveis

### 🔧 Estrutura do projeto

📦 Coworking-Sistema-Reservas
├── 📂 src
│   ├── 📂 Coworking.API         # API .NET 8 (Camada de Apresentação)
	├── 📂 Coworking.Aplication  # API .NET 8 (Casos de Uso e DTOs)
	├── 📂 Coworking.Common      # API .NET 8 (Backend)
	├── 📂 Coworking.Domain      # API .NET 8 (Entidades e Regras de Negócio)
	├── 📂 Coworking.Infra       # API .NET 8 (Repositórios, Migrations)
│   └── 📂 Coworking.Web         # MVC .NET 8 (Frontend/Interface Web)
├── 📂 scripts
│   └── 📄 Inserts.sql           # Dados iniciais
└── 📄 README.md                 # Este arquivo



🧪 Run Tests

- Vá até as pastas "Coworking.Integration" ou "Coworking.Unit"
- Abra o prompt de comando no diretório a rode o seguinte comando

```bash
dotnet test
```

## 👤 Author
[Vinicius Masson](https://www.linkedin.com/in/vinicius-masson/)
Software Developer | .NET
