using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Safe_Notes_Api.Migrations
{
    public partial class UpdatedNoteUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "key",
                table: "Notes");

            migrationBuilder.AddColumn<string>(
                name: "Iv",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iv",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
