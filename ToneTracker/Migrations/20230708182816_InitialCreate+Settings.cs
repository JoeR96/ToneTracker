using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToneTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ControlId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SettingName = table.Column<string>(type: "TEXT", nullable: false),
                    DialId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Setting_Dials_DialId",
                        column: x => x.DialId,
                        principalTable: "Dials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DialId",
                table: "Setting",
                column: "DialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setting");
        }
    }
}
