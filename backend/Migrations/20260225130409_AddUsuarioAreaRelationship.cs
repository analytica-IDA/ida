using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioAreaRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuario_area_IdArea",
                schema: "ida",
                table: "usuario");

            migrationBuilder.DropIndex(
                name: "IX_usuario_IdArea",
                schema: "ida",
                table: "usuario");

            migrationBuilder.DropColumn(
                name: "IdArea",
                schema: "ida",
                table: "usuario");

            migrationBuilder.CreateTable(
                name: "usuario_area",
                schema: "ida",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    IdArea = table.Column<long>(type: "bigint", nullable: false),
                    DtUltimaAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_usuario_area_area_IdArea",
                        column: x => x.IdArea,
                        principalSchema: "ida",
                        principalTable: "area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_area_usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "ida",
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                schema: "ida",
                table: "usuario_area",
                columns: new[] { "Id", "DtUltimaAtualizacao", "IdArea", "IdUsuario" },
                values: new object[] { 1L, new DateTime(2026, 2, 25, 13, 4, 7, 644, DateTimeKind.Utc).AddTicks(6064), 1L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_usuario_area_IdArea",
                schema: "ida",
                table: "usuario_area",
                column: "IdArea");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_area_IdUsuario",
                schema: "ida",
                table: "usuario_area",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuario_area",
                schema: "ida");

            migrationBuilder.AddColumn<long>(
                name: "IdArea",
                schema: "ida",
                table: "usuario",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5117));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5119));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5120));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5121));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5122));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5123));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5126));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5127));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5360));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5328));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5424));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5286));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5394));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5458));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4759));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4764));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4765));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(4767));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5183));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5185));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5186));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5187));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5188));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5189));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5190));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5192));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5193));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5194));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5195));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5195));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5196));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5202));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5202));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5203));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5204));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5205));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5206));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5207));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5208));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5209));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5210));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 2, 21, 20, 29, 54, 807, DateTimeKind.Utc).AddTicks(5211));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "IdArea", "Senha" },
                values: new object[] { new DateTime(2026, 2, 21, 17, 29, 54, 807, DateTimeKind.Local).AddTicks(5512), 1L, "$2a$11$z11Fa2ah3dUFm.KSHnxBseVk4.0D8XYxr75/8lGQkMc4A/v6AxofO" });

            migrationBuilder.CreateIndex(
                name: "IX_usuario_IdArea",
                schema: "ida",
                table: "usuario",
                column: "IdArea");

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_area_IdArea",
                schema: "ida",
                table: "usuario",
                column: "IdArea",
                principalSchema: "ida",
                principalTable: "area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
