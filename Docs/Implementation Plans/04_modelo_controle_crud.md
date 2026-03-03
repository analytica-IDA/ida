# Implementation Plan - ModeloControle and ClienteModeloControle

Implement two new tables for control models and client associations, along with their backend CRUD functionality.

## Proposed Changes

### Database

#### [NEW] [202403031243_AddModeloControle.sql](file:///c:/Portifólio/ida/backend/Migrations/202403031243_AddModeloControle.sql)
- Create table `ida.modelo_controle` with `id` (PK) and `nome`.
- Create table `cliente_modelo_controle` with `id` (PK) and `id_cliente` (FK).
- Seed `ida.modelo_controle` with: 1 (Cadastros), 2 (Varejo), 3 (Saúde).
- Insert "Gerenciamento de Modelo de Controle" into `ida.aplicacao`.
- Link application to "admin" role in `ida.role_aplicacao`.

### Backend

#### [NEW] [ModeloControle.cs](file:///c:/Portifólio/ida/backend/Models/ModeloControle.cs)
- Define the `ModeloControle` entity.

#### [NEW] [ClienteModeloControle.cs](file:///c:/Portifólio/ida/backend/Models/ClienteModeloControle.cs)
- Define the `ClienteModeloControle` entity.

#### [MODIFY] [AppDbContext.cs](file:///c:/Portifólio/ida/backend/Data/AppDbContext.cs)
- Add `DbSet<ModeloControle>` and `DbSet<ClienteModeloControle>`.

#### [NEW] [ModeloControleService.cs](file:///c:/Portifólio/ida/backend/Services/ModeloControleService.cs)
- Implement Business Logic for ModeloControle.

#### [NEW] [ModeloControleController.cs](file:///c:/Portifólio/ida/backend/Controllers/ModeloControleController.cs)
- Implement APIs for ModeloControle.

#### [NEW] [ClienteModeloControleService.cs](file:///c:/Portifólio/ida/backend/Services/ClienteModeloControleService.cs)
- Implement Business Logic for ClienteModeloControle.

#### [NEW] [ClienteModeloControleController.cs](file:///c:/Portifólio/ida/backend/Controllers/ClienteModeloControleController.cs)
- Implement APIs for ClienteModeloControle.

## Verification Plan

### Automated Tests
- Run `dotnet build` to ensure no errors or warnings.
- Test endpoints using `backend.http` or curl.
    - POST `/api/ModeloControle`
    - GET `/api/ModeloControle`
    - PUT `/api/ModeloControle/{id}`
    - DELETE `/api/ModeloControle/{id}`
    - Similar for `ClienteModeloControle`.
    - 
