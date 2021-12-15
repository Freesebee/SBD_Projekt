using Microsoft.EntityFrameworkCore.Migrations;

namespace SBD_Projekt.Migrations
{
    public partial class TotallyFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProduct");

            migrationBuilder.CreateTable(
                name: "FavouriteProduct",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteProduct", x => new { x.ClientId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_FavouriteProduct_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteProduct_ProductId",
                table: "FavouriteProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteProduct");

            migrationBuilder.CreateTable(
                name: "ClientProduct",
                columns: table => new
                {
                    FavouriteProductsId = table.Column<int>(type: "int", nullable: false),
                    ProductEnjoyersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProduct", x => new { x.FavouriteProductsId, x.ProductEnjoyersId });
                    table.ForeignKey(
                        name: "FK_ClientProduct_Clients_ProductEnjoyersId",
                        column: x => x.ProductEnjoyersId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProduct_Products_FavouriteProductsId",
                        column: x => x.FavouriteProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProduct_ProductEnjoyersId",
                table: "ClientProduct",
                column: "ProductEnjoyersId");
        }
    }
}
