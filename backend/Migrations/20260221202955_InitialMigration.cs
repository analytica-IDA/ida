using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ida");

            migrationBuilder.CreateTable(
                name: "aplicacao",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aplicacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "area",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "log_auditoria",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Usuario = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Acao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tabela = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DadosAntigos = table.Column<string>(type: "text", nullable: false),
                    DadosNovos = table.Column<string>(type: "text", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_auditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pessoa",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IdCliente = table.Column<long>(type: "bigint", nullable: true),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pessoa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pessoa_cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "ida",
                        principalTable: "cliente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "cargo",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdRole = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cargo_role_IdRole",
                        column: x => x.IdRole,
                        principalSchema: "ida",
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_aplicacao",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRole = table.Column<long>(type: "bigint", nullable: false),
                    IdAplicacao = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_aplicacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_aplicacao_aplicacao_IdAplicacao",
                        column: x => x.IdAplicacao,
                        principalSchema: "ida",
                        principalTable: "aplicacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_aplicacao_role_IdRole",
                        column: x => x.IdRole,
                        principalSchema: "ida",
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cargo_area",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCargo = table.Column<long>(type: "bigint", nullable: false),
                    IdArea = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargo_area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cargo_area_area_IdArea",
                        column: x => x.IdArea,
                        principalSchema: "ida",
                        principalTable: "area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cargo_area_cargo_IdCargo",
                        column: x => x.IdCargo,
                        principalSchema: "ida",
                        principalTable: "cargo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente_cargo",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCliente = table.Column<long>(type: "bigint", nullable: false),
                    IdCargo = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_cargo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cliente_cargo_cargo_IdCargo",
                        column: x => x.IdCargo,
                        principalSchema: "ida",
                        principalTable: "cargo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cliente_cargo_cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "ida",
                        principalTable: "cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Login = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Senha = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FlAtivo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    IdCargo = table.Column<long>(type: "bigint", nullable: false),
                    IdArea = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_usuario_area_IdArea",
                        column: x => x.IdArea,
                        principalSchema: "ida",
                        principalTable: "area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_cargo_IdCargo",
                        column: x => x.IdCargo,
                        principalSchema: "ida",
                        principalTable: "cargo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_pessoa_Id",
                        column: x => x.Id,
                        principalSchema: "ida",
                        principalTable: "pessoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente_usuario",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCliente = table.Column<long>(type: "bigint", nullable: false),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    IdArea = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cliente_usuario_area_IdArea",
                        column: x => x.IdArea,
                        principalSchema: "ida",
                        principalTable: "area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cliente_usuario_cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "ida",
                        principalTable: "cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cliente_usuario_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "ida",
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificacao",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Mensagem = table.Column<string>(type: "text", nullable: false),
                    Lida = table.Column<bool>(type: "boolean", nullable: false),
                    IdUsuarioDestino = table.Column<long>(type: "bigint", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notificacao_usuario_IdUsuarioDestino",
                        column: x => x.IdUsuarioDestino,
                        principalSchema: "ida",
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5117), "Página Inicial" },
                    { 2L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5119), "Dashboard" },
                    { 3L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5120), "Gerenciamento de Cliente" },
                    { 4L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5121), "Gerenciamento de Pessoa" },
                    { 5L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5122), "Gerenciamento de Cargo" },
                    { 6L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5123), "Gerenciamento de Área" },
                    { 7L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5125), "Gerenciamento de Usuário" },
                    { 8L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5126), "Relatórios" },
                    { 9L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5127), "Configurações" }
                });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "area",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[] { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5360), "Administração" });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "cliente",
                columns: new[] { "Id", "Cnpj", "DtUltimaAtualizacao", "Email", "Foto", "Nome", "Telefone" },
                values: new object[] { 1L, "00000000000000", new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5286), "suporte@analytica.com", null, "Analytica IDA", "00000000000" });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "role",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4759), "admin" },
                    { 2L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4764), "proprietário" },
                    { 3L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4765), "supervisor" },
                    { 4L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4767), "vendedor" }
                });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "cargo",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdRole", "Nome" },
                values: new object[] { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5328), 1L, "Administrador" });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "pessoa",
                columns: new[] { "Id", "Cpf", "DataNascimento", "DtUltimaAtualizacao", "Email", "Foto", "IdCliente", "Nome", "Telefone" },
                values: new object[] { 1L, "00000000000", null, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5458), "suporte@analytica.com", null, 1L, "Administrador", "00000000000" });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "role_aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdAplicacao", "IdRole" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5183), 1L, 1L },
                    { 2L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5185), 2L, 1L },
                    { 3L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5186), 3L, 1L },
                    { 4L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5187), 4L, 1L },
                    { 5L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5188), 5L, 1L },
                    { 6L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5189), 6L, 1L },
                    { 7L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5190), 7L, 1L },
                    { 8L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5192), 8L, 1L },
                    { 9L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5193), 9L, 1L },
                    { 10L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5194), 1L, 2L },
                    { 11L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5195), 2L, 2L },
                    { 12L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5195), 4L, 2L },
                    { 13L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5196), 5L, 2L },
                    { 14L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5202), 6L, 2L },
                    { 15L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5202), 7L, 2L },
                    { 16L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5203), 8L, 2L },
                    { 17L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5204), 1L, 3L },
                    { 18L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5205), 2L, 3L },
                    { 19L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5206), 4L, 3L },
                    { 20L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5207), 7L, 3L },
                    { 21L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5208), 8L, 3L },
                    { 22L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5209), 1L, 4L },
                    { 23L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5210), 2L, 4L },
                    { 24L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5211), 8L, 4L }
                });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "cargo_area",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdArea", "IdCargo" },
                values: new object[] { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5424), 1L, 1L });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "cliente_cargo",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdCargo", "IdCliente" },
                values: new object[] { 1L, new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5394), 1L, 1L });

            migrationBuilder.InsertData(
                schema: "ida",
                table: "usuario",
                columns: new[] { "Id", "DtUltimaAtualizacao", "FlAtivo", "IdArea", "IdCargo", "Login", "Senha" },
                values: new object[] { 1L, new DateTime(2026, 2, 21, 17, 29, 54, 807, DateTimeKind.Local).AddTicks(5512), true, 1L, 1L, "admin", "$2a$11$z11Fa2ah3dUFm.KSHnxBseVk4.0D8XYxr75/8lGQkMc4A/v6AxofO" });

            migrationBuilder.CreateIndex(
                name: "IX_cargo_IdRole",
                schema: "ida",
                table: "cargo",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_cargo_area_IdArea",
                schema: "ida",
                table: "cargo_area",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_cargo_area_IdCargo",
                schema: "ida",
                table: "cargo_area",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_cargo_IdCargo",
                schema: "ida",
                table: "cliente_cargo",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_cargo_IdCliente",
                schema: "ida",
                table: "cliente_cargo",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_usuario_IdArea",
                schema: "ida",
                table: "cliente_usuario",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_usuario_IdCliente",
                schema: "ida",
                table: "cliente_usuario",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_usuario_IdUsuario",
                schema: "ida",
                table: "cliente_usuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_notificacao_IdUsuarioDestino",
                schema: "ida",
                table: "notificacao",
                column: "IdUsuarioDestino");

            migrationBuilder.CreateIndex(
                name: "IX_pessoa_IdCliente",
                schema: "ida",
                table: "pessoa",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_role_aplicacao_IdAplicacao",
                schema: "ida",
                table: "role_aplicacao",
                column: "IdAplicacao");

            migrationBuilder.CreateIndex(
                name: "IX_role_aplicacao_IdRole",
                schema: "ida",
                table: "role_aplicacao",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_IdArea",
                schema: "ida",
                table: "usuario",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_IdCargo",
                schema: "ida",
                table: "usuario",
                column: "IdCargo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cargo_area",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "cliente_cargo",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "cliente_usuario",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "log_auditoria",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "notificacao",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "role_aplicacao",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "usuario",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "aplicacao",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "area",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "cargo",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "pessoa",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "role",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "cliente",
                schema: "ida");
        }
    }
}
