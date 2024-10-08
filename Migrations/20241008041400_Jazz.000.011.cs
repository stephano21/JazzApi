using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JazzApi.Migrations
{
    /// <inheritdoc />
    public partial class Jazz000011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "CAT",
                columns: table => new
                {
                    IdDevice = table.Column<Guid>(type: "uuid", nullable: false),
                    UniqueId = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: false),
                    SystemVersion = table.Column<string>(type: "text", nullable: false),
                    BatteryLevel = table.Column<float>(type: "real", nullable: true),
                    IsCharging = table.Column<bool>(type: "boolean", nullable: true),
                    IsRooted = table.Column<bool>(type: "boolean", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    LocationPermissionStatus = table.Column<string>(type: "text", nullable: false),
                    CameraPermissionStatus = table.Column<string>(type: "text", nullable: false),
                    NotificationPermissionStatus = table.Column<string>(type: "text", nullable: false),
                    ConnectionType = table.Column<string>(type: "text", nullable: false),
                    IsConnected = table.Column<bool>(type: "boolean", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.IdDevice);
                    table.ForeignKey(
                        name: "FK_Devices_Profile_UserId",
                        column: x => x.UserId,
                        principalSchema: "AUTH",
                        principalTable: "Profile",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                schema: "CAT",
                table: "Devices",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices",
                schema: "CAT");
        }
    }
}
