using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateLancamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lancamento",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    id_modelo_controle = table.Column<long>(type: "bigint", nullable: false),
                    data_lancamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lancamento_modelo_controle_id_modelo_controle",
                        column: x => x.id_modelo_controle,
                        principalSchema: "ida",
                        principalTable: "modelo_controle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lancamento_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "ida",
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lancamento_cadastro",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    qtd_click_link = table.Column<long>(type: "bigint", nullable: false),
                    qtd_cadastros = table.Column<long>(type: "bigint", nullable: false),
                    vlr_ticket_medio = table.Column<decimal>(type: "numeric(38,2)", nullable: false),
                    vlr_investimento_meta = table.Column<decimal>(type: "numeric(38,2)", nullable: false),
                    vlr_investimento_google = table.Column<decimal>(type: "numeric(38,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamento_cadastro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lancamento_cadastro_lancamento_Id",
                        column: x => x.Id,
                        principalSchema: "ida",
                        principalTable: "lancamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lancamento_saude",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    qtd_click_meta = table.Column<long>(type: "bigint", nullable: false),
                    qtd_click_google = table.Column<long>(type: "bigint", nullable: false),
                    qtd_contatos_reais = table.Column<long>(type: "bigint", nullable: false),
                    qtd_conversao_consultas = table.Column<long>(type: "bigint", nullable: false),
                    vlr_ticket_medio_consultas = table.Column<decimal>(type: "numeric(38,2)", nullable: true),
                    qtd_entrada_redes_sociais = table.Column<long>(type: "bigint", nullable: false),
                    qtd_entrada_google = table.Column<long>(type: "bigint", nullable: false),
                    vlr_investimento_meta = table.Column<decimal>(type: "numeric(38,12)", nullable: true),
                    vlr_investimento_google = table.Column<decimal>(type: "numeric(38,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamento_saude", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lancamento_saude_lancamento_Id",
                        column: x => x.Id,
                        principalSchema: "ida",
                        principalTable: "lancamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lancamento_varejo",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    qtd_atendimento = table.Column<long>(type: "bigint", nullable: false),
                    qtd_fechamento = table.Column<long>(type: "bigint", nullable: false),
                    faturamento = table.Column<decimal>(type: "numeric(38,2)", nullable: false),
                    qtd_instagram = table.Column<long>(type: "bigint", nullable: false),
                    qtd_facebook = table.Column<long>(type: "bigint", nullable: false),
                    qtd_google = table.Column<long>(type: "bigint", nullable: false),
                    qtd_indicacao = table.Column<long>(type: "bigint", nullable: false),
                    vlr_investimento_meta = table.Column<decimal>(type: "numeric(38,2)", nullable: false),
                    vlr_investimento_google = table.Column<decimal>(type: "numeric(38,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamento_varejo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lancamento_varejo_lancamento_Id",
                        column: x => x.Id,
                        principalSchema: "ida",
                        principalTable: "lancamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6010));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6012));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6014));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6015));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6016));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6018));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6019));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6020));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6022));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6023));

            migrationBuilder.InsertData(
                schema: "ida",
                table: "aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[] { 11L, new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6024), "Gerenciamento de Lançamentos" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6577));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6504));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6686));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6417));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6628));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6095));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6098));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6100));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6736));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(5553));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(5560));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(5562));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(5563));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6157));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6160));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6161));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6162));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6163));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6165));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6166));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6280));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6282));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6285));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6287));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6288));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6290));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6291));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6292));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6294));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6297));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6298));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6299));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6301));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6302));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6305));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6306));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6307));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 25L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6284));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "Senha" },
                values: new object[] { new DateTime(2026, 3, 4, 18, 35, 20, 312, DateTimeKind.Local).AddTicks(6838), "$2a$11$4lcKKjQuzLOhmwgOfqGgj.5wjErCze5RPhPVxa6BLMzqQ41PSNqxa" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 4, 21, 35, 20, 534, DateTimeKind.Utc).AddTicks(3654));

            migrationBuilder.InsertData(
                schema: "ida",
                table: "role_aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdAplicacao", "IdRole" },
                values: new object[,]
                {
                    { 26L, new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6295), 11L, 2L },
                    { 27L, new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6303), 11L, 3L },
                    { 28L, new DateTime(2026, 3, 4, 21, 35, 20, 312, DateTimeKind.Utc).AddTicks(6309), 11L, 4L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_id_modelo_controle",
                schema: "ida",
                table: "lancamento",
                column: "id_modelo_controle");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_id_usuario",
                schema: "ida",
                table: "lancamento",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lancamento_cadastro",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "lancamento_saude",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "lancamento_varejo",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "lancamento",
                schema: "ida");

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8801));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8804));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8806));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8808));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8810));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8812));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8814));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8816));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8818));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8820));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9355));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9293));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9467));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9415));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8914));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8918));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8920));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9522));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8374));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8383));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8386));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8388));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8987));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8991));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8993));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8995));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8998));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9000));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9002));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9017));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9021));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9022));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9024));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9026));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9028));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9029));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9031));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9033));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9034));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9036));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9037));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9039));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9041));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9042));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9044));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 25L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9019));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "Senha" },
                values: new object[] { new DateTime(2026, 3, 3, 13, 7, 44, 796, DateTimeKind.Local).AddTicks(9619), "$2a$11$Rs0IeiGLCsCe3C8fERMn/.vfr/XXrRJlBD8UIGlYrt1ygXvTsRyQG" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 3, 16, 7, 45, 81, DateTimeKind.Utc).AddTicks(4883));
        }
    }
}
