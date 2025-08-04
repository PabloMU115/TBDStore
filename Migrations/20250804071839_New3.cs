using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TBD.Migrations
{
    /// <inheritdoc />
    public partial class New3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fechaEnviado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "fechaPedido",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "paypalID",
                table: "Pedidos");

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaEnviado",
                table: "Ordenes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaPedido",
                table: "Ordenes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "paypalID",
                table: "Ordenes",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fechaEnviado",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "fechaPedido",
                table: "Ordenes");

            migrationBuilder.DropColumn(
                name: "paypalID",
                table: "Ordenes");

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaEnviado",
                table: "Pedidos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fechaPedido",
                table: "Pedidos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "paypalID",
                table: "Pedidos",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
