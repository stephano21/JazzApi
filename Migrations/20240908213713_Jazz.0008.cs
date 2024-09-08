using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz0008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SyncCode",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyncCode",
                schema: "AUTH",
                table: "Profile");
        }
    }
}
