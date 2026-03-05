# Usuários de Teste - Módulo Lançamentos

Abaixo estão os 6 usuários criados pelo script `Docs/seed_test_data.sql` para facilitar a validação em diversos papéis.

**Login para todos:** `<login>`
**Senha para todos:** `abc123`

---

### Cliente: Mega Varejo Ltda (Modelo Varejo)

| Nome / Perfil | Cargo / Role | Login |
| ------------- | ------------ | ----- |
| **Carlos Roberto** | Diretor Executivo (Proprietário) | `carlos.roberto` |
| **Ana Paula** | Supervisor de Vendas (Supervisor) | `ana.paula` |
| **Pedro Almeida** | Consultor de Vendas (Vendedor) | `pedro.almeida` |

---

### Cliente: Clínica Vida Saudável (Modelo Saúde)

| Nome / Perfil | Cargo / Role | Login |
| ------------- | ------------ | ----- |
| **Dr. Roberto Silva** | Diretor Executivo (Proprietário) | `roberto.silva` |
| **Mariana Souza** | Atendente Especializado (Vendedor)| `mariana.souza` |

---

### Cliente: StartApp Digital (Modelo Cadastros)

| Nome / Perfil | Cargo / Role | Login |
| ------------- | ------------ | ----- |
| **Lucas Mendes** | Gestor de Tráfego (Vendedor) | `lucas.mendes` |

---

*Nota: O script inseriu dados nas tabelas relacionadas mantendo coesão entre Cliente -> Cliente_Usuario -> Usuario -> Pessoa.*
