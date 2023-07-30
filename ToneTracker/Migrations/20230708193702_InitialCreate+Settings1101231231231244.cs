using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToneTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSettings1101231231231244 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Dials_ControlId",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Toggles_ControlId",
                table: "Setting");

            migrationBuilder.RenameColumn(
                name: "ControlId",
                table: "Setting",
                newName: "ToggleId");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_ControlId",
                table: "Setting",
                newName: "IX_Setting_ToggleId");

            migrationBuilder.AddColumn<Guid>(
                name: "DialId",
                table: "Setting",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DialId",
                table: "Setting",
                column: "DialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Dials_DialId",
                table: "Setting",
                column: "DialId",
                principalTable: "Dials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting",
                column: "ToggleId",
                principalTable: "Toggles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Dials_DialId",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_DialId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "DialId",
                table: "Setting");

            migrationBuilder.RenameColumn(
                name: "ToggleId",
                table: "Setting",
                newName: "ControlId");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_ToggleId",
                table: "Setting",
                newName: "IX_Setting_ControlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Dials_ControlId",
                table: "Setting",
                column: "ControlId",
                principalTable: "Dials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Toggles_ControlId",
                table: "Setting",
                column: "ControlId",
                principalTable: "Toggles",
                principalColumn: "Id");
        }
    }
}
