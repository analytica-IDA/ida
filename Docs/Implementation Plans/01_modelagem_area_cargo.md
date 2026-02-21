# Nova Modelagem de Área e Cargo

Este é o plano de implementação para refatorar as tabelas `Area` e `Cargo` e suas respectivas telas.

## Alterações no Banco de Dados (Backend)

### 1. Modelos
- **`Area.cs`**:
  - Remover a propriedade `IdCliente` e `Cliente`.
  - Adicionar propriedade de navegação `ICollection<CargoArea> CargosAreas`.
- **`Cargo.cs`**:
  - Adicionar propriedade de navegação `ICollection<ClienteCargo> ClientesCargos`.
  - Adicionar propriedade de navegação `ICollection<CargoArea> CargosAreas`.
- **Criar `ClienteCargo.cs`**:
  - Entidade de relacionamento contendo `IdCliente`, `Cliente`, `IdCargo`, `Cargo` e herdando de `BaseEntity`.
- **Criar `CargoArea.cs`**:
  - Entidade de relacionamento contendo `IdCargo`, `Cargo`, `IdArea`, `Area` e herdando de `BaseEntity`.

### 2. AppDbContext.cs
- Adicionar `DbSet<ClienteCargo>` e `DbSet<CargoArea>`.
- Configurar as chaves estrangeiras no método `OnModelCreating` para as novas tabelas de relacionamento.
- Remover as configurações antigas do `IdCliente` na `Area`.

### 3. Migrations
- Gerar e aplicar a migração no Entity Framework para refletir as mudanças no banco.

---

## Alterações na Lógica (Backend)

### Controladores e Repositórios
- **`AreaController.cs`**:
  - Ao criar/editar uma área, receber na requisição a lista (ou o único ID) de Cargo a ser vinculado na tabela `CargoArea`.
  - O retorno do GET de Áreas deve trazer o(s) Cargo(s) relacionado(s) para ser exibido na tela, se necessário.
- **`CargoController.cs`**:
  - Ao criar/editar um cargo, receber na requisição o ID do cliente a ser vinculado na tabela `ClienteCargo`.
  - O retorno do GET de Cargos deve incluir os clientes vinculados para facilitar a edição no frontend.
- **`UserController.cs` / `Me`**:
  - Garantir que um usuário que faz login possua em seu token ou perfil a indicação de qual Cliente ele pertence de forma clara, ou retornar as informações do cliente logado no endpoint `/user/me`. Atualmente, `ClienteUsuario` faz esse vínculo e pode ser utilizado.

---

## Alterações no Frontend

### 1. Tela de Gerenciamento de Cargos (`CargosPage.tsx`)
- Adicionar campo *Cliente* no formulário (Modal) de Cargo.
- Lógica de exibição baseada na Role do usuário:
  - Se a Role for **"proprietário"**: O campo deve ser preenchido ou usar automaticamente o Cliente no qual ele está vinculado. O endpoint deve deduzir o cliente no backend através da identidade do usuário ou o front passar o ID oculto/bloqueado.
  - Se a Role for **"admin"**: Exibir o `<select>` para que o admin possa buscar e vincular qualquer Cliente.
- Ajustar chamadas à API em `POST /cargo` e `PUT /cargo/:id` enviando o id do cliente para a tabela `ClienteCargo`.

### 2. Tela de Gerenciamento de Áreas (`AreasPage.tsx`)
- Remover o campo "Empresa / Cliente" da tela de cadastro de Área, pois o relacionamento foi migrado para Cargo via `CargoArea`.
- Adicionar um campo para selecionar o(s) **Cargo(s)** (`IdCargo`) ao qual a Área está sendo vinculada.
- Ajustar as chamadas `POST /area` e `PUT /area/:id` para trafegar a informação do(s) cargo(s) selecionado(s).

## Verification Plan

### Testes Automatizados (Build / Lint)
- Verificar compilação do Backend (`dotnet build`). Não deve conter erros nem avisos.
- Verificar execução do EF Migrations.

### Verificação Manual
- Entrar como `admin` e criar um Cargo alocando um `Cliente` específico. 
- Entrar como `proprietário` e confirmar que, ao criar Cargo, ele já herda/está forçado para o Cliente atual (Testar vinculação transparente).
- Na tela de Áreas, remover as referências a Cliente. Adicionar uma nova Área vinculada a um sub-cargo criado.
- Assegurar que os Menus de Cadastro Rápido (QuickCliente/QuickCargo, etc.) seguem o Padrão Visual.
