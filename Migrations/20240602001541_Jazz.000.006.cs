using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz000006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Usuario",
                schema: "AUTH",
                table: "Log",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                schema: "AUTH",
                table: "Log",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Ambiente",
                schema: "AUTH",
                table: "Log",
                newName: "Environment");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "CAT",
                table: "TaskNotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                schema: "CAT",
                table: "TaskNotes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                schema: "CAT",
                table: "TaskNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                schema: "CAT",
                table: "TaskNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateIP",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUser",
                schema: "CAT",
                table: "TaskNotes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "CreateIP",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "DeleteIP",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "DeleteUser",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "UpdateIP",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.DropColumn(
                name: "UpdateUser",
                schema: "CAT",
                table: "TaskNotes");

            migrationBuilder.RenameColumn(
                name: "User",
                schema: "AUTH",
                table: "Log",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "Environment",
                schema: "AUTH",
                table: "Log",
                newName: "Ambiente");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "AUTH",
                table: "Log",
                newName: "Fecha");
        }
    }
}
