using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Safe_Notes_Api.Migrations
{
    public partial class noteupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Notes",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Notes",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<bool>(
                name: "isPublic",
                table: "Notes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "isPublic",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "title",
                table: "Notes");
        }
    }
}
