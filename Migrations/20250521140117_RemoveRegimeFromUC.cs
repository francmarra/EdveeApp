using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edveeeeeee.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRegimeFromUC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atividades",
                table: "UCs");

            migrationBuilder.DropColumn(
                name: "Avaliacao",
                table: "UCs");

            migrationBuilder.DropColumn(
                name: "Competencias",
                table: "UCs");

            migrationBuilder.DropColumn(
                name: "Conteudos",
                table: "UCs");

            migrationBuilder.DropColumn(
                name: "Regime",
                table: "UCs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Atividades",
                table: "UCs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Avaliacao",
                table: "UCs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Competencias",
                table: "UCs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Conteudos",
                table: "UCs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Regime",
                table: "UCs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
