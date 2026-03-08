using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestmentControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vlr_investimento_google",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropColumn(
                name: "vlr_investimento_meta",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropColumn(
                name: "vlr_investimento_google",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropColumn(
                name: "vlr_investimento_meta",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropColumn(
                name: "vlr_investimento_google",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.DropColumn(
                name: "vlr_investimento_meta",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.AddColumn<long>(
                name: "id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_varejo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_varejo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_saude",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_saude",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_cadastro",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_cadastro",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cliente_investimento_google",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cliente = table.Column<long>(type: "bigint", nullable: false),
                    vlr_investimento_google = table.Column<decimal>(type: "numeric(38,2)", precision: 38, scale: 2, nullable: false, defaultValue: 0m),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_investimento_google", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cliente_investimento_google_cliente_id_cliente",
                        column: x => x.id_cliente,
                        principalSchema: "ida",
                        principalTable: "cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente_investimento_meta",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_cliente = table.Column<long>(type: "bigint", nullable: false),
                    vlr_investimento_meta = table.Column<decimal>(type: "numeric(38,2)", precision: 38, scale: 2, nullable: false, defaultValue: 0m),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_investimento_meta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cliente_investimento_meta_cliente_id_cliente",
                        column: x => x.id_cliente,
                        principalSchema: "ida",
                        principalTable: "cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7766));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7767));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7768));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7769));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7770));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7772));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7773));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7775));

            migrationBuilder.InsertData(
                schema: "ida",
                table: "aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[] { 12L, new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7776), "Gerenciamento de Investimentos" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(8024));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7991));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(8108));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7946));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(8083));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7823));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7826));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7827));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(8135));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7526));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7530));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7531));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7532));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7865));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7868));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7868));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7869));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7870));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7871));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7876));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7876));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7877));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7880));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7881));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7881));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7882));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7883));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7884));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7884));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7886));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7887));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7887));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7888));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7889));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7890));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7891));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7892));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 25L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7878));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 26L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7885));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 27L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7889));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 28L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7892));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 29L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7879));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "Senha" },
                values: new object[] { new DateTime(2026, 3, 8, 15, 33, 45, 191, DateTimeKind.Local).AddTicks(8178), "$2a$11$eTZFk9JgfWf86mQjyYCrUeNsx2V1fr5F/qLzuHDscQ/L9YfSjUFZm" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 633, DateTimeKind.Utc).AddTicks(190));

            migrationBuilder.InsertData(
                schema: "ida",
                table: "role_aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdAplicacao", "IdRole" },
                values: new object[] { 30L, new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7879), 12L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_varejo_id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_varejo",
                column: "id_cliente_investimento_google");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_varejo_id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_varejo",
                column: "id_cliente_investimento_meta");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_saude_id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_saude",
                column: "id_cliente_investimento_google");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_saude_id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_saude",
                column: "id_cliente_investimento_meta");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_cadastro_id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_cadastro",
                column: "id_cliente_investimento_google");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_cadastro_id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_cadastro",
                column: "id_cliente_investimento_meta");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_investimento_google_id_cliente",
                schema: "ida",
                table: "cliente_investimento_google",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_investimento_meta_id_cliente",
                schema: "ida",
                table: "cliente_investimento_meta",
                column: "id_cliente");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamento_cadastro_cliente_investimento_google_id_cliente_~",
                schema: "ida",
                table: "lancamento_cadastro",
                column: "id_cliente_investimento_google",
                principalSchema: "ida",
                principalTable: "cliente_investimento_google",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamento_cadastro_cliente_investimento_meta_id_cliente_in~",
                schema: "ida",
                table: "lancamento_cadastro",
                column: "id_cliente_investimento_meta",
                principalSchema: "ida",
                principalTable: "cliente_investimento_meta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamento_saude_cliente_investimento_google_id_cliente_inv~",
                schema: "ida",
                table: "lancamento_saude",
                column: "id_cliente_investimento_google",
                principalSchema: "ida",
                principalTable: "cliente_investimento_google",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamento_saude_cliente_investimento_meta_id_cliente_inves~",
                schema: "ida",
                table: "lancamento_saude",
                column: "id_cliente_investimento_meta",
                principalSchema: "ida",
                principalTable: "cliente_investimento_meta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamento_varejo_cliente_investimento_google_id_cliente_in~",
                schema: "ida",
                table: "lancamento_varejo",
                column: "id_cliente_investimento_google",
                principalSchema: "ida",
                principalTable: "cliente_investimento_google",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lancamento_varejo_cliente_investimento_meta_id_cliente_inve~",
                schema: "ida",
                table: "lancamento_varejo",
                column: "id_cliente_investimento_meta",
                principalSchema: "ida",
                principalTable: "cliente_investimento_meta",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lancamento_cadastro_cliente_investimento_google_id_cliente_~",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.DropForeignKey(
                name: "FK_lancamento_cadastro_cliente_investimento_meta_id_cliente_in~",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.DropForeignKey(
                name: "FK_lancamento_saude_cliente_investimento_google_id_cliente_inv~",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropForeignKey(
                name: "FK_lancamento_saude_cliente_investimento_meta_id_cliente_inves~",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropForeignKey(
                name: "FK_lancamento_varejo_cliente_investimento_google_id_cliente_in~",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropForeignKey(
                name: "FK_lancamento_varejo_cliente_investimento_meta_id_cliente_inve~",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropTable(
                name: "cliente_investimento_google",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "cliente_investimento_meta",
                schema: "ida");

            migrationBuilder.DropIndex(
                name: "IX_lancamento_varejo_id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropIndex(
                name: "IX_lancamento_varejo_id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropIndex(
                name: "IX_lancamento_saude_id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropIndex(
                name: "IX_lancamento_saude_id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropIndex(
                name: "IX_lancamento_cadastro_id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.DropIndex(
                name: "IX_lancamento_cadastro_id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 12L);

            migrationBuilder.DropColumn(
                name: "id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropColumn(
                name: "id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_varejo");

            migrationBuilder.DropColumn(
                name: "id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropColumn(
                name: "id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_saude");

            migrationBuilder.DropColumn(
                name: "id_cliente_investimento_google",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.DropColumn(
                name: "id_cliente_investimento_meta",
                schema: "ida",
                table: "lancamento_cadastro");

            migrationBuilder.AddColumn<decimal>(
                name: "vlr_investimento_google",
                schema: "ida",
                table: "lancamento_varejo",
                type: "numeric(38,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "vlr_investimento_meta",
                schema: "ida",
                table: "lancamento_varejo",
                type: "numeric(38,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "vlr_investimento_google",
                schema: "ida",
                table: "lancamento_saude",
                type: "numeric(38,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "vlr_investimento_meta",
                schema: "ida",
                table: "lancamento_saude",
                type: "numeric(38,12)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "vlr_investimento_google",
                schema: "ida",
                table: "lancamento_cadastro",
                type: "numeric(38,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "vlr_investimento_meta",
                schema: "ida",
                table: "lancamento_cadastro",
                type: "numeric(38,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4391));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4392));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4392));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4393));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4394));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4395));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4395));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4396));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4397));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4397));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4476));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4784));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4743));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4845));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4673));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4815));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4536));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4538));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4539));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4874));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4097));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4101));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4102));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4102));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4575));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4576));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4577));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4577));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4578));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4579));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4579));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4584));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4585));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4587));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4587));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4588));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4589));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4589));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4590));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4591));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4592));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4592));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4593));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4594));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4594));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4596));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4596));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4597));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 25L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4585));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 26L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4591));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 27L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4595));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 28L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4597));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 29L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 368, DateTimeKind.Utc).AddTicks(4586));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "Senha" },
                values: new object[] { new DateTime(2026, 3, 4, 21, 27, 53, 368, DateTimeKind.Local).AddTicks(4918), "$2a$11$BCrRwL/i0H6IRaDU.KBYF.OEgCZfX8wvt1/UXvIxODZY5J5wX2nuK" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 5, 0, 27, 53, 748, DateTimeKind.Utc).AddTicks(5670));
        }
    }
}
