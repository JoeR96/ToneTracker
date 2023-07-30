using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToneTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSettings1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Dials_DialId",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_DialId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "DialId",
                table: "Setting");

            migrationBuilder.AlterColumn<Guid>(
                name: "ControlId",
                table: "Setting",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "ToggleId",
                table: "Setting",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Setting_ControlId",
                table: "Setting",
                column: "ControlId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_ToggleId",
                table: "Setting",
                column: "ToggleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Dials_ControlId",
                table: "Setting",
                column: "ControlId",
                principalTable: "Dials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting",
                column: "ToggleId",
                principalTable: "Toggles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Dials_ControlId",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_ControlId",
                table: "Setting");

            migrationBuilder.DropIndex(
                name: "IX_Setting_ToggleId",
                table: "Setting");

            migrationBuilder.DropColumn(
                name: "ToggleId",
                table: "Setting");

            migrationBuilder.AlterColumn<Guid>(
                name: "ControlId",
                table: "Setting",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

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
        }
    }
}
