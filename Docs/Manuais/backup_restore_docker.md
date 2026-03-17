# Manual de Backup e Restore de Banco de Dados no Docker

Este documento detalha os passos necessários para realizar o **backup** e o **restore** de um banco de dados que está sendo executado em um container Docker. Os exemplos abaixo utilizam o **PostgreSQL** como referência, pois é um dos bancos de dados mais comuns, mas a lógica se aplica a outros bancos (como MySQL, trocando `pg_dump`/`psql` por `mysqldump`/`mysql`).

---

## 1. Como fazer o Backup (Dump)

Para realizar o backup, utilizamos o utilitário de exportação do banco de dados de dentro do container e direcionamos a saída para a nossa máquina local (host).

### Método 1: Exportando diretamente para a máquina host (Recomendado para `.sql`)
Este é o método mais simples. Ele executa o `pg_dump` no container e salva o arquivo `.sql` diretamente na pasta em que você está no seu terminal.

```bash
docker exec -t <nome_do_container> pg_dump -U <usuario> -d <nome_do_banco> -c -O > ./meu_backup.sql
```
*Observação: As flags `-c` (clean) e `-O` (no-owner) são úteis para facilitar o restore em outros ambientes.*

### Método 2: Criando o backup dentro do container e copiando para o host
Ideal para formatos customizados (compressed) do Postgres.

1. Crie o dump dentro do container:
   ```bash
   docker exec -t <nome_do_container> pg_dump -U <usuario> -d <nome_do_banco> -F c -f /tmp/meu_backup.dump
   ```
2. Copie o arquivo do container para a sua máquina:
   ```bash
   docker cp <nome_do_container>:/tmp/meu_backup.dump ./meu_backup.dump
   ```

---

## 2. Como fazer o Restore

O processo de restore depende do formato em que o backup foi gerado (`.sql` puro ou formato `.dump`).

### Restore a partir de um arquivo `.sql` (texto puro)
Se você gerou o backup usando o Método 1 (arquivo `.sql`), basta ler o arquivo local e enviar para o comando `psql` dentro do container:

```bash
cat ./meu_backup.sql | docker exec -i <nome_do_container> psql -U <usuario> -d <nome_do_banco>
```
*Atenção: Note o uso de `-i` (interactive) ao invés de `-t` (tty) no `docker exec` para que o redirecionamento de entrada (`<` ou `cat |`) funcione corretamente.*

> **No Windows (PowerShell):** Se o comando `cat` der problemas com codificação (encoding), utilize o redirecionamento nativo:
> ```powershell
> Get-Content ./meu_backup.sql | docker exec -i <nome_do_container> psql -U <usuario> -d <nome_do_banco>
> ```
> Ou executando o comando diretamente no CMD:
> ```cmd
> docker exec -i <nome_do_container> psql -U <usuario> -d <nome_do_banco> < ./meu_backup.sql
> ```

### Restore a partir de um arquivo `.dump` (formato customizado)
Se o backup foi feito usando a flag `-F c` no PostgreSQL, use o `pg_restore`.

1. Primeiro, copie o arquivo de backup da sua máquina para o container:
   ```bash
   docker cp ./meu_backup.dump <nome_do_container>:/tmp/meu_backup.dump
   ```
2. Execute o `pg_restore` dentro do container:
   ```bash
   docker exec -t <nome_do_container> pg_restore -U <usuario> -d <nome_do_banco> -1 -e /tmp/meu_backup.dump
   ```
   *(A flag `-1` faz o restore em uma única transação, e `-e` para no primeiro erro).*

---

## Dicas Importantes

- **Descobrindo o nome do container:** Use `docker ps` para listar os containers ativos e identificar o `<nome_do_container>` do seu banco de dados.
- **Lidando com senhas:** Caso o banco exija senha e não confie na conexão local, você pode passar a variável de ambiente no comando. Exemplo:
  ```bash
  docker exec -e PGPASSWORD="sua_senha_aqui" -i <nome_do_container> psql -U <usuario> -d <nome_do_banco>
  ```
- **Limpando o banco antes do restore:** Se o banco já tiver tabelas e você estiver restaurando um `.sql` que não possui comandos `DROP TABLE` ou `CLEAN`, é recomendado recriar o schema ou as tabelas antes de restaurar para evitar conflitos de dados.
