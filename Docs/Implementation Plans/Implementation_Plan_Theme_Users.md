# Implementation Plan - Theme & User Management Fixes

This plan addresses the broken theme toggle, replaces mock user data with database data, and adds the user registration functionality.

## Proposed Changes

### Frontend

#### [MODIFY] [index.css](file:///c:/Portifolio/analytica/ida/frontend/src/index.css)
- Adjust the Tailwind v4 configuration to properly support the `.dark` class strategy.
- Ensure the `:root.dark` and `.dark` selectors are correctly set up for Tailwind's utility classes.

#### [MODIFY] [UsersPage.tsx](file:///c:/Portifolio/analytica/ida/frontend/src/pages/UsersPage.tsx)
- Integrate `react-query` to fetch users from `api/user`.
- Replace hardcoded table rows with dynamic data from the API.
- Implement a "Create User" modal using `framer-motion` for animations.
- Add a form inside the modal to capture: Name, Login, Email, Password, and Role.
- Connect the form to the `api/user/register` endpoint.

### Backend

#### [MODIFY] [UserController.cs](file:///c:/Portifolio/analytica/ida/backend/Controllers/UserController.cs)
- Implement `GET /api/user`: Returns a list of all users with their associated personal data, role, and area.
- Implement `GET /api/user/roles`: Returns available roles for selecting in the frontend.
- Fully implement `POST /api/user/register`:
    - Create a new `Pessoa` entry.
    - Create a new `Usuario` entry linked to the `Pessoa`.
    - Hash the password (using `IAuthService`).
    - Validate permissions of the current logged-in user.

## Verification Plan

### Automated Tests
- Run `dotnet build` in `backend` to ensure no errors or warnings were introduced.
- (Optional) Create a simple integration test for `UserController`.

### Manual Verification
1.  **Theme Toggle**: Open the application, click the theme toggle in the sidebar, and verify that the UI colors change correctly (background, text, cards).
2.  **User List**: Navigate to the "Usuários" page and verify that "Paulo Takeda" (from `DbSeeder`) and any newly created users are visible.
3.  **User Registration**:
    - Click "Criar Novo Usuário".
    - Fill in the form.
    - Submit and verify the success message.
    - Verify the new user appears in the list.
    - Attempt to login with the new user credentials.
