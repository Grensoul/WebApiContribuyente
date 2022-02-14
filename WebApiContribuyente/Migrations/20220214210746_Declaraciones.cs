using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiContribuyente.Migrations
{
    public partial class Declaraciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Declaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RFC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContribuyenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Declaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Declaciones_Contribuyentes_ContribuyenteId",
                        column: x => x.ContribuyenteId,
                        principalTable: "Contribuyentes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Declaciones_ContribuyenteId",
                table: "Declaciones",
                column: "ContribuyenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Declaciones");
        }
    }
}
