using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz000008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskNotes_Profile_UserId",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdateUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdateIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeleteUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeleteIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreateIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoupleId",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SyncCode",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StackTrace",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestTraceIdentifier",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestID",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Plataform",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "API",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "API");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InnerException",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Environment",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile",
                column: "CoupleId",
                principalSchema: "AUTH",
                principalTable: "Profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNotes_Profile_UserId",
                schema: "CAT",
                table: "TaskNotes",
                column: "UserId",
                principalSchema: "AUTH",
                principalTable: "Profile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskNotes_Profile_UserId",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "SyncCode",
                schema: "AUTH",
                table: "Profile");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DeleteUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DeleteIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreateUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreateIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NickName",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CoupleId",
                schema: "AUTH",
                table: "Profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "StackTrace",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RequestTraceIdentifier",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RequestID",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Plataform",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                defaultValue: "API",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "API");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "InnerException",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Environment",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                schema: "AUTH",
                table: "Log",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Profile_CoupleId",
                schema: "AUTH",
                table: "Profile",
                column: "CoupleId",
                principalSchema: "AUTH",
                principalTable: "Profile",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNotes_Profile_UserId",
                schema: "CAT",
                table: "TaskNotes",
                column: "UserId",
                principalSchema: "AUTH",
                principalTable: "Profile",
                principalColumn: "UserId");
        }
    }
}
