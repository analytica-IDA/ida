# Implementação de Isolamento Multitenant e Refatoramento de Menus

Objetivo: Isolar a listagem de registros por Cliente (Multitenancy básico) com base no vínculo da Pessoa logada e aplicar as restrições rígidas de Menus solicitadas pelo usuário para as roles "proprietário", "supervisor" e "vendedor".

## User Review Required

Confirmar se a regra de negócio do multitenancy está alinhada ao que foi pedido: Administradores (`idRole = 1`) continuam visualizando tudo, enquanto as outras Roles listam apenas registros de `Pessoa`, `Cargo`, `Área` e `Usuário` que pertencem ao mesmo `IdCliente` do usuário logado na API.

## Proposed Changes

### Banco de Dados & Modelos (Backend)

#### [MODIFY] [Pessoa.cs](file:///c:/Portifolio/analytica/ida/backend/Models/Pessoa.cs)
- Adição da Foreign Key `IdCliente` (long).
- Adição da Navigation Property virtual `Cliente`.

#### [MODIFY] [AppDbContext.cs](file:///c:/Portifolio/analytica/ida/backend/Data/AppDbContext.cs)
- Configuração do relacionamento 1:N no `OnModelCreating` entre `Cliente` -> `Pessoa`.
- Ajuste no `.HasData()` da `Pessoa` do Admin para fixá-la ao Cliente "Analytica IDA" (`IdCliente = 1`).
- Refatoramento total do bloco `.HasData()` da tabela `RoleAplicacao` para garantir o menu exato:
  - **Proprietário**: Página Inicial, Dashboard, Pessoas, Cargos, Áreas, Usuários, Relatórios (Sem Cliente e Config).
  - **Supervisor**: Página Inicial, Dashboard, Pessoas, Usuários, Relatórios.
  - **Vendedor**: Página Inicial, Dashboard, Relatórios.

### API de Autenticação e Multitenancy

#### [MODIFY] [AuthService.cs](file:///c:/Portifolio/analytica/ida/backend/Services/AuthService.cs)
- No momento da geração do Token (`GenerateToken`), precisamos olhar para a propriedade `usuario.Pessoa.IdCliente` e embuti-la como uma Claim (`idCliente`). Assim, saberemos exatamente a qual empresa o token logado pertence sem consultar o banco a cada requisição.

#### [MODIFY] [UserController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/UserController.cs)
- Retornar o `idCliente` no JSON do endpoint `/user/me` para o Frontend poder usar nos forms e lógicas dinâmicas.

### Restrição de Registros nos Controladores (Isolamento de Tenant)

Para os Controladores de **Pessoa**, **Cargo**, **Área** e **Usuário**:
- Identificar se o usuário chamador não é `admin` (leitura da claim `roleId != 1`).
- Recuperar a claim `idCliente`.
- [GET]: Filtrar resultados com `.Where(x => x.IdCliente == idCliente)`. No caso de Área (que se liga a cargo), `.Where(a => a.CargosAreas.Any(ca => ca.Cargo.IdCliente == idCliente))`.
- [POST/PUT]: Forçar que as entidades geradas nasçam amarradas ao `idCliente` do usuário (substituindo o que vem do payload para evitar *Insecure Direct Object Reference* - IDOR).

### Interfaces Gráficas (Frontend)

#### [MODIFY] [PessoasPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/PessoasPage.tsx)
- Ocultar a caixa de seleção de "Cliente Vinculado" se o `userProfile.role` não for `admin` (ou pré-preencher com o dele via read-only).
- Igualar a inteligência de envio do `idCliente` silenciosamente igual já fizemos na tela de Cargos.

## Verification Plan

### Automated Tests
1. Gerar e atualizar a Migration. O banco vai falhar se a Pessoa não puder receber `IdCliente` obrigatório (teremos que dropar antes).

### Manual Verification
1. Dropar esquema via `docker exec`, gerar/aplicar a migração `InitialMigration`.
2. Fazer Login como `admin`, criar um Cliente A, associar a uma nova Pessoa X, e vinculá-la ao Cargo de "vendedor" e "proprietário".
3. Logar com o Proprietário recém-criado, abrir a tela de Pessoas. Confirmar que ele cria pessoas e lista SOMENTE pessoas daquela base.
4. Ao abrir a barra lateral do Proprietário, os menus de Cargo/Área estarão visíveis, mas o Cliente e Config não.
5. Logar com Vendedor: ver se a barra exibe somente Dashboard, Home e Relatórios.
