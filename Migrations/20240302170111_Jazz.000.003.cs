using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz000003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "AUTH",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "AUTH",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Bloqueo",
                schema: "AUTH",
                table: "Users",
                newName: "PasswordExpired");

            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                schema: "AUTH",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Porfile",
                schema: "AUTH",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    NickName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Porfile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Porfile_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "AUTH",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Porfile",
                schema: "AUTH");

            migrationBuilder.DropColumn(
                name: "Lock",
                schema: "AUTH",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PasswordExpired",
                schema: "AUTH",
                table: "Users",
                newName: "Bloqueo");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "AUTH",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "AUTH",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
