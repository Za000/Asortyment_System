using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asortyment_System.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asortyments",
                columns: table => new
                {
                    EAN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    kodProduktu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nazwaProduktu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cena = table.Column<float>(type: "real", nullable: false),
                    stanMagazynowy = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EAN", x => x.EAN);
                });

            migrationBuilder.CreateTable(
                name: "ConnectedEAN",
                columns: table => new
                {
                    LinkedEAN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BaseEAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    AsortymentEAN = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedEAN", x => x.LinkedEAN);
                    table.ForeignKey(
                        name: "FK_ConnectedEAN_Asortyments_AsortymentEAN",
                        column: x => x.AsortymentEAN,
                        principalTable: "Asortyments",
                        principalColumn: "EAN");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedEAN_AsortymentEAN",
                table: "ConnectedEAN",
                column: "AsortymentEAN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedEAN");

            migrationBuilder.DropTable(
                name: "Asortyments");
        }
    }
}
