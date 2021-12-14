using Microsoft.EntityFrameworkCore.Migrations;

namespace SBD_Projekt.Migrations
{
    public partial class SimpleRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ad717d87-2c3f-4a97-acc2-c1378999fdb5", "e5b1d25c-8fec-4f02-9b4c-c67a7b6a2172", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "33f7ce6b-fda0-479c-872d-41cffdeee917", "c5d91ea8-dc44-4378-98e4-bdac6cef331a", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33f7ce6b-fda0-479c-872d-41cffdeee917");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad717d87-2c3f-4a97-acc2-c1378999fdb5");
        }
    }
}
