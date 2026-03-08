# Implementation Plan - Investment Control Modeling

This plan outlines the changes required to implement the Investment Control feature, including new database tables, backend CRUD operations, and frontend UI updates.

## User Review Required

> [!IMPORTANT]
> The existing `vlr_investimento_meta` and `vlr_investimento_google` fields in the `lancamento_varejo`, `lancamento_cadastro`, and `lancamento_saude` tables will be replaced by foreign keys to the new `cliente_investimento_meta` and `cliente_investimento_google` tables. This is a breaking change for the database schema.

## Proposed Changes

### Database & Models

#### [NEW] [ClienteInvestimentoMeta.cs](file:///c:/Portif%C3%B3lio/ida/backend/Models/ClienteInvestimentoMeta.cs)
- Create model with `Id` (long, PK), `IdCliente` (long, FK), `VlrInvestimentoMeta` (decimal), and `DtUltimaAtualizacao`.

#### [NEW] [ClienteInvestimentoGoogle.cs](file:///c:/Portif%C3%B3lio/ida/backend/Models/ClienteInvestimentoGoogle.cs)
- Create model with `Id` (long, PK), `IdCliente` (long, FK), `VlrInvestimentoGoogle` (decimal), and `DtUltimaAtualizacao`.

#### [MODIFY] [LancamentoVarejo.cs](file:///c:/Portif%C3%B3lio/ida/backend/Models/LancamentoVarejo.cs)
#### [MODIFY] [LancamentoCadastro.cs](file:///c:/Portif%C3%B3lio/ida/backend/Models/LancamentoCadastro.cs)
#### [MODIFY] [LancamentoSaude.cs](file:///c:/Portif%C3%B3lio/ida/backend/Models/LancamentoSaude.cs)
- Replace `VlrInvestimentoMeta` and `VlrInvestimentoGoogle` fields with `IdClienteInvestimentoMeta` and `IdClienteInvestimentoGoogle` (long?).

#### [MODIFY] [AppDbContext.cs](file:///c:/Portif%C3%B3lio/ida/backend/Data/AppDbContext.cs)
- Add `DbSet` for the new models.
- Configure table names and relationships.
- Update `SeedData` to include the "Gerenciamento de Investimentos" application and link it to the "admin" role.

---

### Backend API

#### [NEW] [ClienteInvestimentoMetaController.cs](file:///c:/Portif%C3%B3lio/ida/backend/Controllers/ClienteInvestimentoMetaController.cs)
#### [NEW] [ClienteInvestimentoGoogleController.cs](file:///c:/Portif%C3%B3lio/ida/backend/Controllers/ClienteInvestimentoGoogleController.cs)
- Implement full CRUD for the new investment tables.

#### [MODIFY] [LancamentoDtos.cs](file:///c:/Portif%C3%B3lio/ida/backend/DTOs/LancamentoDtos.cs)
- Update DTOs to reflect the change from values to IDs.

#### [MODIFY] [LancamentoController.cs](file:///c:/Portif%C3%B3lio/ida/backend/Controllers/LancamentoController.cs)
- Update creation/update logic to automatically link launches to the latest active investment record for the client if none is provided.

---

### Frontend

#### [NEW] [InvestimentosPage.tsx](file:///c:/Portif%C3%B3lio/ida/frontend/src/pages/InvestimentosPage.tsx)
- Develop a screen to manage investment records per client (Admin only).

#### [MODIFY] [LancamentosPage.tsx](file:///c:/Portif%C3%B3lio/ida/frontend/src/pages/LancamentosPage.tsx)
- Update the launch modal:
    - Investment fields become read-only for sellers.
    - Fetch and display current investment values for the selected client.
    - Submit the investment IDs with the launch data.

#### [MODIFY] [App.tsx](file:///c:/Portif%C3%B3lio/ida/frontend/src/App.tsx)
- Register the new route and add it to the navigation menu.

## Verification Plan

### Automated Tests
- No existing automated tests were found that cover this specific flow. I will verify via manual API calls if needed.

### Manual Verification
1. **Database Migration**:
   - Run `dotnet ef migrations add AddInvestmentControl` in the `backend` directory.
   - Run `dotnet ef database update`.
2. **Backend CRUD**:
   - Use the Swagger UI (if available) or `curl`/Postman to test the new CRUD endpoints for `ClienteInvestimentoMeta` and `ClienteInvestimentoGoogle`.
3. **Frontend Flow**:
   - Log in as **Admin**:
     - Create a client investment record.
     - Verify it appears in the new management screen.
   - Log in as **Vendedor**:
     - Go to the Launch screen.
     - Open the "Novo Lançamento" modal.
     - Verify that investment fields are populated and read-only.
     - Save a launch and verify it's correctly saved in the database with the linked IDs.
