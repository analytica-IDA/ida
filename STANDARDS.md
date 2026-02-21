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
