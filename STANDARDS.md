# Projeto Analytica IDA - Padronizações

Para manter a integridade e qualidade do projeto, todos os desenvolvedores (incluindo IAs) devem seguir estas regras:

1. **Erro Zero**: Nunca deixar erros em nenhum arquivo do projeto (Lint, TypeScript ou Runtime).
2. **Build Limpo**: O backend deve sempre compilar sem nenhum **Aviso (Warning)** e nenhum **Erro**.
3. **Execução Pura**: Nunca deixar rodando nada (processos dotnet, node, etc) após uma codificação, a menos que solicitado.
4. **Permissões Totais**: Não há necessidade de pedir autorização para executar comandos ou ferramentas; você tem acesso total para resolver os problemas.
5. **Naming**: Seguir as convenções de nomenclatura em Português para entidades e propriedades (PascalCase).
6. **CORS**: Garantir comunicação entre Porta 3000 e Porta 5100.
7. **Planos de Implementação**: Sempre que criar um plano de implementação, pode executar sem pedir revisão. Salve esses arquivos `.md` na pasta `Docs/Implementation Plans`.
8. **Navegação de Cadastro**: Toda tela de gerenciamento que for utilizada por outra tela deve ter um botão para criação de um novo registro no local onde é utilizada, respeitando a hierarquia de roles.
9. **Registro de Telas**: Toda tela nova criada deve ser registrada na tabela `Aplicacao` do banco de dados.
10. **Compromisso Git**: Nunca fazer o commit e o push no git por conta própria, sempre espere o pedido explícito do usuário.
11. **Padrão Visual (Clean Design)**:
    - **Cores Primárias**: Azul (#2563eb), Cinza Escuro (#171717).
    - **Modo Claro**: Fundo: #f9fafb (neutral-50), Cards: #ffffff, Texto: #171717 (neutral-900).
    - **Modo Escuro**: Fundo: #0a0a0a (neutral-950), Cards: #171717 (neutral-900), Texto: #f5f5f5 (neutral-100).
    - **Status**: Sucesso: Emerald-600, Erro: Red-600, Alerta: Amber-600.
    - **Restrição**: Evitar o uso de cores vibrantes variadas (roxo, rosa, índigo) para manter a sobriedade.
12. **Não mexer em nada no docker diretamente, dropar base, de maneira alguma**.