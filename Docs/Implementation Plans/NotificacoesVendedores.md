# Sistema de Notificação de Atividade do Vendedor

## Objetivos
- Notificar `admin`, `proprietário` e `supervisor` sobre as atividades realizadas por usuários com a role `vendedor`.
- Utilizar a tabela existente `Notificacao` para armazenar os avisos, permitindo controle de leitura (lida/não lida) para cada destinatário.

## Proposed Changes

### Backend
1. **AuditService.cs (`c:\Portifolio\analytica\ida\backend\Services\AuditService.cs`)**
   - Modificar `LogAction` para:
     a) Buscar o `Usuario` através do parâmetro `usuario` (Login) juntamente com suas roles e a Empresa (Pessoa.IdCliente).
     b) Verificar se a role é `vendedor`. Se sim, criar `Notificacao` para os administradores, proprietários e supervisores da empresa desse usuário.
2. **Controllers/NotificacaoController.cs** e **Services/NotificacaoService.cs**
   - Criar para servir as notificações ao Frontend (listar, marcar lida).

### Frontend
1. **Header/Navbar**
   - Adicionar o ícone de sino (Bell) `lucide-react`.
   - Criar Dropdown de notificações, chamando API para buscar e ler.
2. **Api Queries**
   - `useQuery` para notificações não lidas e `useMutation` para marcar lida.

## Verification Plan
### Automated Tests
- Compilar o backend usando `dotnet build`.
- Build do frontend `npm run build`.

### Manual Verification
- Testar atividades com `vendedor` e recebimento como `admin` ou `proprietário`.
