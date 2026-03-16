## LR.CodeAIGenerate

API de exemplo em .NET 10 focada em **persistência de pessoas**, **validação com FluentValidation**, **acesso a dados com Entity Framework Core (SQLite)** e **autenticação JWT**, estruturada em camadas (`Domain`, `Business`, `Data`, `Api`) e com testes de domínio em `LR.CodeAIGenerate.Domain.Tests`.

### Estrutura dos projetos

- **LR.CodeAIGenerate.Domain**  
  - Entidades de domínio (por exemplo, `Pessoa`) e regras centrais.
- **LR.CodeAIGenerate.Business**  
  - Interfaces de repositório e lógica de negócio (ex.: `IRepositorioPessoa` e derivados).
- **LR.CodeAIGenerate.Data**  
  - Implementações de repositório, `AppDbContext` (EF Core), migrações e `PessoaValidator` usando FluentValidation.
- **LR.CodeAIGenerate.Api**  
  - API ASP.NET Core (`Program.cs`, controllers) com autenticação JWT, autorização por policies e configuração de Swagger/OpenAPI (Swashbuckle).
- **LR.CodeAIGenerate.Domain.Tests**  
  - Testes automatizados de domínio com xUnit, FluentAssertions e coverlet (cobertura).

### Tecnologias principais

- **.NET**: `net10.0`
- **API Web**: ASP.NET Core (`Microsoft.NET.Sdk.Web`)
- **Banco de dados**: SQLite (`Microsoft.EntityFrameworkCore.Sqlite`)
- **ORM**: Entity Framework Core 10 (`DbContext` `AppDbContext`)
- **Validação**: FluentValidation
- **Autenticação**: JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **OpenAPI / Swagger**: `Microsoft.AspNetCore.OpenApi` + `Swashbuckle.AspNetCore`
- **Testes**: xUnit, FluentAssertions, coverlet

### Configuração de ambiente

1. **Instalar SDK .NET 10** (preview ou versão que você estiver usando no projeto).  
2. **Restaurar dependências** na pasta raiz da solução:

   ```bash
   dotnet restore
   ```

3. **Configurar connection string (opcional)**  
   - Por padrão, o `AppDbContext` usa SQLite com:
     - Connection string padrão no `Program.cs`: `Data Source=app.db`
     - Ou o valor de `ConnectionStrings:DefaultConnection` no `appsettings.json` da API.

4. **Configurar JWT (opcional)**  
   - Em `Program.cs` há configurações padrão:
     - `Jwt:Issuer` (padrão: `LR.CodeAIGenerate`)
     - `Jwt:Audience` (padrão: `LR.CodeAIGenerate.Audience`)
     - `Jwt:Secret` (padrão: `chave-super-secreta-nao-usar-em-producao-12345`)
   - Para uso em produção, defina esses valores em `appsettings.*.json` ou variáveis de ambiente.

### Como rodar a API

Na raiz do repositório, execute:

```bash
dotnet build
dotnet run --project LR.CodeAIGenerate.Api/LR.CodeAIGenerate.Api.csproj
```

Por padrão, a API sobe em `https://localhost:5001` (ou porta definida pelo Kestrel/launchSettings).  

### Swagger / OpenAPI

- Em ambiente de **desenvolvimento**, o Swagger está habilitado em `Program.cs`.
- Após subir a API, acesse:
  - `https://localhost:5001/swagger` (ajuste a porta conforme seu ambiente)
- O endpoint JSON do documento OpenAPI normalmente estará em:
  - `/swagger/v1/swagger.json`

### Migrações e banco de dados (EF Core)

O projeto `LR.CodeAIGenerate.Data` já referencia EF Core e ferramentas de migração. Exemplos de comandos (na raiz da solução):

```bash
dotnet ef migrations add NomeDaMigracao -p LR.CodeAIGenerate.Data/LR.CodeAIGenerate.Data.csproj -s LR.CodeAIGenerate.Api/LR.CodeAIGenerate.Api.csproj
dotnet ef database update -p LR.CodeAIGenerate.Data/LR.CodeAIGenerate.Data.csproj -s LR.CodeAIGenerate.Api/LR.CodeAIGenerate.Api.csproj
```

> **Observação**: para usar o `dotnet ef`, instale a ferramenta global se ainda não tiver:
>
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### Testes automatizados

Para rodar os testes do projeto de domínio:

```bash
dotnet test LR.CodeAIGenerate.Domain.Tests/LR.CodeAIGenerate.Domain.Tests.csproj
```

Isso executa os testes xUnit e coleta cobertura via coverlet (`coverlet.collector`).

### Autenticação e autorização

- A API utiliza **JWT Bearer** (`JwtBearerDefaults.AuthenticationScheme`) configurado em `Program.cs`.
- Há uma policy de exemplo:
  - `add-pessoa`: exige usuário autenticado e claim `scope` com valor `pessoa.add`.
- Endpoints protegidos devem ser anotados com:
  - `[Authorize(Policy = "add-pessoa")]` (ou outras policies que você criar).

### Convenções gerais

- **Camadas bem separadas**:
  - `Domain`: modelos e regras de negócio puras.
  - `Business`: interfaces e contratos de repositório/serviços.
  - `Data`: infraestrutura de dados, repositórios concretos e validações.
  - `Api`: exposição HTTP, controllers e configuração de middleware.
- **Validação**: feita com `PessoaValidator` via FluentValidation antes de persistir dados (`RepositorioPessoa`).

### Próximos passos sugeridos

- Adicionar documentação dos endpoints no XML (comentários) e integrar com Swagger.
- Criar exemplos de requests/responses no Swagger para facilitar uso da API.
- Completar controllers (por exemplo, CRUD completo de `Pessoa`) e testes de integração para a API.

