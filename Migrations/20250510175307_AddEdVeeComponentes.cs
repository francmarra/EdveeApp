using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edveeeeeee.Migrations
{
    /// <inheritdoc />
    public partial class AddEdVeeComponentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadeCurricularId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atividades_UCs_UnidadeCurricularId",
                        column: x => x.UnidadeCurricularId,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadeCurricularId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_UCs_UnidadeCurricularId",
                        column: x => x.UnidadeCurricularId,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Competencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadeCurricularId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competencias_UCs_UnidadeCurricularId",
                        column: x => x.UnidadeCurricularId,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conteudos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadeCurricularId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conteudos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conteudos_UCs_UnidadeCurricularId",
                        column: x => x.UnidadeCurricularId,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_UnidadeCurricularId",
                table: "Atividades",
                column: "UnidadeCurricularId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_UnidadeCurricularId",
                table: "Avaliacoes",
                column: "UnidadeCurricularId");

            migrationBuilder.CreateIndex(
                name: "IX_Competencias_UnidadeCurricularId",
                table: "Competencias",
                column: "UnidadeCurricularId");

            migrationBuilder.CreateIndex(
                name: "IX_Conteudos_UnidadeCurricularId",
                table: "Conteudos",
                column: "UnidadeCurricularId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atividades");

            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "Competencias");

            migrationBuilder.DropTable(
                name: "Conteudos");
        }
    }
}
