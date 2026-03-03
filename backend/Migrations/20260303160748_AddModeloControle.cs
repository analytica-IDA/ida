using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddModeloControle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "modelo_controle",
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
                    table.PrimaryKey("PK_modelo_controle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cliente_modelo_controle",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCliente = table.Column<long>(type: "bigint", nullable: false),
                    IdModeloControle = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_modelo_controle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cliente_modelo_controle_cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalSchema: "ida",
                        principalTable: "cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cliente_modelo_controle_modelo_controle_IdModeloControle",
                        column: x => x.IdModeloControle,
                        principalSchema: "ida",
                        principalTable: "modelo_controle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                schema: "ida",
                table: "aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[] { 10L, new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8820), "Gerenciamento de Modelo de Controle" });

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

            migrationBuilder.InsertData(
                schema: "ida",
                table: "modelo_controle",
                columns: new[] { "Id", "DtUltimaAtualizacao", "Nome" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8914), "Cadastros" },
                    { 2L, new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8918), "Varejo" },
                    { 3L, new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(8920), "Saúde" }
                });

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

            migrationBuilder.InsertData(
                schema: "ida",
                table: "role_aplicacao",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdAplicacao", "IdRole" },
                values: new object[] { 25L, new DateTime(2026, 3, 3, 16, 7, 44, 796, DateTimeKind.Utc).AddTicks(9019), 10L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_cliente_modelo_controle_IdCliente",
                schema: "ida",
                table: "cliente_modelo_controle",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_modelo_controle_IdModeloControle",
                schema: "ida",
                table: "cliente_modelo_controle",
                column: "IdModeloControle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cliente_modelo_controle",
                schema: "ida");

            migrationBuilder.DropTable(
                name: "modelo_controle",
                schema: "ida");

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7897));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7898));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7899));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7900));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7901));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7902));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7903));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7904));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7905));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(8117));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(8086));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(8167));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(8045));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(8140));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(8190));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7674));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7678));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7679));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7680));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7947));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7949));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7949));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7950));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7951));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7951));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7952));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7953));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7954));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7954));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7955));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7956));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7962));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7963));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7963));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7964));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7965));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7966));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7966));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7967));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7968));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7969));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7970));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 508, DateTimeKind.Utc).AddTicks(7970));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "Senha" },
                values: new object[] { new DateTime(2026, 2, 25, 10, 4, 7, 508, DateTimeKind.Local).AddTicks(8223), "$2a$11$nXITcqG8EvxWXBF3jXU4MeIuEdAjDMY7CveWkAPcZBRzkQOnvGvdi" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 25, 13, 4, 7, 644, DateTimeKind.Utc).AddTicks(6064));
        }
    }
}
