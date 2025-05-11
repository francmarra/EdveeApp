using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edveeeeeee.Migrations
{
    /// <inheritdoc />
    public partial class AddLigacoesEdVee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ligacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadeCurricularId = table.Column<int>(type: "int", nullable: false),
                    OrigemTipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrigemId = table.Column<int>(type: "int", nullable: false),
                    DestinoTipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ligacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ligacoes_UCs_UnidadeCurricularId",
                        column: x => x.UnidadeCurricularId,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ligacoes_UnidadeCurricularId",
                table: "Ligacoes",
                column: "UnidadeCurricularId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ligacoes");
        }
    }
}
