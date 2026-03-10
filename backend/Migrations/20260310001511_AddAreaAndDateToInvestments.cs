using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaAndDateToInvestments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_referencia",
                schema: "ida",
                table: "cliente_investimento_meta",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<long>(
                name: "id_area",
                schema: "ida",
                table: "cliente_investimento_meta",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_referencia",
                schema: "ida",
                table: "cliente_investimento_google",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<long>(
                name: "id_area",
                schema: "ida",
                table: "cliente_investimento_google",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4431));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4432));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4433));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4434));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4434));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4435));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4436));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4436));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4437));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4438));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4439));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4439));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4845));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4791));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cargo_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4729));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "cliente_cargo",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4878));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4498));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4502));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "modelo_controle",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4503));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "pessoa",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4958));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4040));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4045));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4046));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4047));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4556));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 2L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4558));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 3L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4559));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 4L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4560));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 5L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4604));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 6L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4605));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 7L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4609));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 8L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4610));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 9L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4610));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 10L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4613));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 11L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4613));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4614));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 13L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4615));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 14L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4615));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 15L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4616));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 16L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4617));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 17L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4618));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 18L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4619));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 19L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4619));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 20L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4620));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 21L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4621));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 22L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4622));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 23L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4623));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 24L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4623));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 25L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4611));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 26L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4617));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 27L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4621));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 28L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4624));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 29L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4612));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 30L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 80, DateTimeKind.Utc).AddTicks(4612));

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DtUltimaAtualizacao", "Senha" },
                values: new object[] { new DateTime(2026, 3, 9, 21, 15, 9, 80, DateTimeKind.Local).AddTicks(5003), "$2a$11$Bi6Pfbm0x3KldUnPdiATD.KpJS1iWq7joSTJ4BtFSfgfO8N6Oc9vm" });

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "usuario_area",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 10, 0, 15, 9, 188, DateTimeKind.Utc).AddTicks(7426));

            migrationBuilder.CreateIndex(
                name: "IX_cliente_investimento_meta_id_area",
                schema: "ida",
                table: "cliente_investimento_meta",
                column: "id_area");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_investimento_google_id_area",
                schema: "ida",
                table: "cliente_investimento_google",
                column: "id_area");

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_investimento_google_area_id_area",
                schema: "ida",
                table: "cliente_investimento_google",
                column: "id_area",
                principalSchema: "ida",
                principalTable: "area",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_cliente_investimento_meta_area_id_area",
                schema: "ida",
                table: "cliente_investimento_meta",
                column: "id_area",
                principalSchema: "ida",
                principalTable: "area",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cliente_investimento_google_area_id_area",
                schema: "ida",
                table: "cliente_investimento_google");

            migrationBuilder.DropForeignKey(
                name: "FK_cliente_investimento_meta_area_id_area",
                schema: "ida",
                table: "cliente_investimento_meta");

            migrationBuilder.DropIndex(
                name: "IX_cliente_investimento_meta_id_area",
                schema: "ida",
                table: "cliente_investimento_meta");

            migrationBuilder.DropIndex(
                name: "IX_cliente_investimento_google_id_area",
                schema: "ida",
                table: "cliente_investimento_google");

            migrationBuilder.DropColumn(
                name: "data_referencia",
                schema: "ida",
                table: "cliente_investimento_meta");

            migrationBuilder.DropColumn(
                name: "id_area",
                schema: "ida",
                table: "cliente_investimento_meta");

            migrationBuilder.DropColumn(
                name: "data_referencia",
                schema: "ida",
                table: "cliente_investimento_google");

            migrationBuilder.DropColumn(
                name: "id_area",
                schema: "ida",
                table: "cliente_investimento_google");

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

            migrationBuilder.UpdateData(
                schema: "ida",
                table: "aplicacao",
                keyColumn: "Id",
                keyValue: 12L,
                column: "DtUltimaAtualizacao",
                value: new DateTime(2026, 3, 8, 18, 33, 45, 191, DateTimeKind.Utc).AddTicks(7776));

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
                table: "role_aplicacao",
                keyColumn: "Id",
                keyValue: 30L,
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
        }
    }
}
