using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz000007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoupleId",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile",
                column: "CoupleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile",
                column: "CoupleId",
                principalSchema: "AUTH",
                principalTable: "Profile",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "CoupleId",
                schema: "AUTH",
                table: "Profile");
        }
    }
}
