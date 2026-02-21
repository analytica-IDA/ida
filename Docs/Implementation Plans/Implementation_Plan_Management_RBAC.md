# Implementation Plan - Management Systems & RBAC

This plan details the implementation of full management screens for Pessoas, Clientes, Cargos, and Areas, along with a dynamic Frontend RBAC system.

## Proposed Changes

### Database & Seed
- **[MODIFY] [DbSeeder.cs](file:///c:/Portifolio/analytica/ida/backend/Data/Seeders/DbSeeder.cs)**:
    - Add new `Aplicacao` records: "Gerenciamento de Pessoas", "Gerenciamento de Clientes", "Gerenciamento de Cargos", "Gerenciamento de Áreas".
    - Link these new applications to the `admin` role in `role_aplicacao`.

### Backend Controllers
- **[NEW] [PessoaController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/PessoaController.cs)**: Full CRUD for personal data.
- **[NEW] [ClienteController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/ClienteController.cs)**: Full CRUD for clients.
- **[NEW] [CargoController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/CargoController.cs)**: Full CRUD for occupations.
- **[NEW] [AreaController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/AreaController.cs)**: Full CRUD for functional areas.
- **[MODIFY] [UserController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/UserController.cs)**:
    - Add `GET /api/user/menu` to return list of `Aplicacao` assigned to the user's role.

### Frontend RBAC & Layout
- **[MODIFY] [api.ts](file:///c:/Portifolio/analytica/ida/frontend/src/services/api.ts)**: Ensure consistency and error handling for common CRUD operations.
- **[MODIFY] [DashboardLayout.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/layouts/DashboardLayout.tsx)**:
    - Fetch menu items from `api/user/menu`.
    - Map application names to appropriate Lucide icons and routes.
    - Remove hardcoded sidebar items.

### Frontend Management Pages
- **[NEW] [PessoasPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/PessoasPage.tsx)**: Management table + creation modal.
- **[NEW] [ClientesPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/ClientesPage.tsx)**: Management table + creation modal.
- **[NEW] [CargosPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/CargosPage.tsx)**: Management table + creation modal.
- **[NEW] [AreasPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/AreasPage.tsx)**: Management table + creation modal.

### Integration (Rule 8)
- **[MODIFY] [UsersPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/UsersPage.tsx)**:
    - Add "Quick Create" buttons (plus icons) next to `Cargo` and `Area` dropdowns in the registration modal.
    - These buttons will trigger the respective creation logic.

## Verification Plan
1. **Database Check**: Run seeder and verify `Aplicacoes` table.
2. **Backend**: Verify all new CRUD endpoints via Swagger or Postman (if available) or during frontend testing.
3. **RBAC**: Login with different users (if available) or verify that only apps linked to the user's role appear in the sidebar.
4. **CRUD Testing**: Create, read, update, and delete a record for each new entity.
5. **Quick Create**: Open `UsersPage` -> "Criar Novo Usuário" -> Click "+" on Cargo/Area -> Create new -> Verify it appears in the dropdown immediately.
