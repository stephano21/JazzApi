using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz000002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "SEG",
                newName: "Users",
                newSchema: "AUTH");

            migrationBuilder.RenameTable(
                name: "UserLogin",
                schema: "SEG",
                newName: "UserLogin",
                newSchema: "AUTH");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SEG");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "AUTH",
                newName: "Users",
                newSchema: "SEG");

            migrationBuilder.RenameTable(
                name: "UserLogin",
                schema: "AUTH",
                newName: "UserLogin",
                newSchema: "SEG");
        }
    }
}
