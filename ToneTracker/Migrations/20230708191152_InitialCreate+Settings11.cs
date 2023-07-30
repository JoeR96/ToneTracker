using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToneTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSettings11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_ToggleId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "ToggleId",
                table: "Setting");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Toggles_ControlId",
                table: "Setting",
                column: "ControlId",
                principalTable: "Toggles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Toggles_ControlId",
                table: "Setting");

            migrationBuilder.AddColumn<Guid>(
                name: "ToggleId",
                table: "Setting",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Setting_ToggleId",
                table: "Setting",
                column: "ToggleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting",
                column: "ToggleId",
                principalTable: "Toggles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
