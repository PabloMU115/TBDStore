using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TBD.Migrations
{
    /// <inheritdoc />
    public partial class new4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "numeroDeGuia",
                table: "Ordenes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numeroDeGuia",
                table: "Ordenes");
        }
    }
}
