# Implementation Plan - User & Client Flow Refinement

This plan addresses the requirement of registering a `Pessoa` before a `Usuario` and implementing the relationship between `Cliente`, `Usuario`, and `Area`.

## User Review Required
> [!IMPORTANT]
> I am adding `IdArea` to the `ClienteUsuario` table. This field will track which Area a User belongs to *within* a specific Client's context.

## Proposed Changes

### Database & Models

#### [MODIFY] [ClienteUsuario.cs](file:///c:/Portifolio/analytica/ida/backend/Models/ClienteUsuario.cs)
- Add `public long IdArea { get; set; }`
- Add `public Area? Area { get; set; }`

#### [MODIFY] [AppDbContext.cs](file:///c:/Portifolio/analytica/ida/backend/Data/AppDbContext.cs)
- Update `ClienteUsuario` configuration to include the relationship with `Area`.

### Backend Controllers

#### [MODIFY] [UserController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/UserController.cs)
- Update `Register` logic:
    - Instead of receiving `Pessoa` details (Nome, CPF, etc.), it should receive `IdPessoa`.
    - Ensure the `Id` of the `Usuario` is set to the selected `IdPessoa`.

#### [MODIFY] [ClienteController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/ClienteController.cs)
- Add `GET /api/cliente/{id}/usuarios`: Returns all users associated with a client and their areas.
- Add `POST /api/cliente/{id}/usuarios`: Associates a user and an area to a client.
- Add `DELETE /api/cliente/{id}/usuarios/{userId}`: Removes an association.

### Frontend Pages

#### [MODIFY] [UsersPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/UsersPage.tsx)
- Update `RegisterModal`:
    - Replace the "Personal Info" fields with a single `Pessoa` selection dropdown.
    - Add a `+` button next to the `Pessoa` dropdown that opens a "Quick Pessoa Create" modal (similar to Cargos/Areas).
    - This ensures a `Pessoa` is always created/selected first.

#### [MODIFY] [ClientesPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/ClientesPage.tsx)
- Add an "Gerenciar Usuários" button to each client row.
- Create a `ClientUserModal` that:
    - Lists currently associated users and their areas.
    - Allows adding a new user (select from all users) and assigning an area (select from all areas).

## Verification Plan
1. **Model Check**: Verify DB context and entity fields.
2. **User Flow**:
    - Try to create a user. Verify you must select a person.
    - Use the "Quick Create" for Person, then select them for the user.
3. **Client Flow**:
    - Open "Gerenciar Usuários" for a client.
    - Link a user to an area.
    - Verify the list updates.
