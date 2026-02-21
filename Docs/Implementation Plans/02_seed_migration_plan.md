# Refatoração Inicialização de Banco (HasData Migration)

Objetivo: Transferir a carga de dados iniciais do sistema ("seed") do arquivo `DbSeeder.cs` (que executamos dinamicamente via backend) para a base estrutural do Entity Framework, ou seja, diretamente dentro do `OnModelCreating` no `AppDbContext.cs` através do `HasData()`. Em sequência, recriar a database via arquivo unificado de migração.

## User Review Required

Nenhuma alteração de negócio impactante que fuja ao que já foi providenciado até agora. Estaremos "chumbando" as informações já estipuladas no último prompt (Criação de Applicacoes base, Roles e Usuário admin/Analytica).

## Proposed Changes

### Backend/Data
Centralização e controle fixo da migração.

#### [MODIFY] [AppDbContext.cs](file:///c:/Portifolio/analytica/ida/backend/Data/AppDbContext.cs)
- Adição da Fluent API `HasData()` em cima das seguintes entidades (respeitando chaves primárias pré-fixadas):
  - `Roles`: admin, proprietário, supervisor, vendedor
  - `Aplicacao`: Gerenciamento de Pessoa, Usuário, Cliente, Cargo, Área, Dashboard, Relatórios, Configurações
  - `RoleAplicacao`: Vínculo cruzando o ID da Role "admin" com as IDs geradas de **todas** as Aplicações
  - `Cliente`: Cadastro do Cliente Genérico "Analytica IDA" com os dados primários estipulados
  - `Cargo`: Criação do Cargo "Administrador" (vinculado à Role id do "admin")
  - `Area`: Criação da Área "Administração"
  - `ClienteCargo` e `CargoArea`: Vinculando Área de Administração para o Cargo Administrador, que por sua vez vincula ao Cliente Analytica
  - `Pessoa`: Criação da Pessoa "Administrador"
  - `Usuario`: Criação do Usuário "admin" acoplado aos IDs acima e sua respectiva Hash de senha no formato Bcrypt (`$2a$11$0z5Q1zO8XTYJ9Zp...` para "T4k3d4@@!").

#### [MODIFY] [DbSeeder.cs](file:///c:/Portifolio/analytica/ida/backend/Data/Seeders/DbSeeder.cs)
- O arquivo será esvaziado de contexto, contendo apenas o `context.Database.Migrate()` para disparar a atualização ao invés de construir os dados.  

A geração das senhas hash será feita com geradores estáticos compatíveis com BCrypt do BCrypt.Net para podermos injetar o string literal lá sem causar dependência excessiva em compile-time do `AppDbContext.cs`.

## Verification Plan

### Automated Tests
1. Gerar arquivo limpo final com `dotnet ef migrations add InitialMigration --context AppDbContext`.
2. Após o Drop (`docker exec ...`), rodar `dotnet ef database update`. O EF deve passar com aviso `0`.

### Manual Verification
1. Logar no PgAdmin e verificar se todas as tabelas descritas na aba "Data" apontam diretamente os registros.
2. Iniciar a API Local (`dotnet run`).
3. Fazer login no FrontEnd utilizando `admin` e `T4k3d4@@!` e garantir que o menu apareça com todas as telas renderizadas e as configs do Role acopladas corretamente.
