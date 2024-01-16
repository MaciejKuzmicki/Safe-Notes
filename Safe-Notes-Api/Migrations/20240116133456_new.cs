using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Safe_Notes_Api.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "iv",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iv",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "key",
                table: "Notes");
        }
    }
}
