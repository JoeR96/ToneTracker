using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToneTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSettings1101231231231244123123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Dials_DialId",
                table: "Setting");

            migrationBuilder.DropForeignKey(
                name: "FK_Setting_Toggles_ToggleId",
                table: "Setting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Setting",
                table: "Setting");

            migrationBuilder.RenameTable(
                name: "Setting",
                newName: "Settings");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_ToggleId",
                table: "Settings",
                newName: "IX_Settings_ToggleId");

            migrationBuilder.RenameIndex(
                name: "IX_Setting_DialId",
                table: "Settings",
                newName: "IX_Settings_DialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Dials_DialId",
                table: "Settings",
                column: "DialId",
                principalTable: "Dials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Toggles_ToggleId",
                table: "Settings",
                column: "ToggleId",
                principalTable: "Toggles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Dials_DialId",
                table: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Toggles_ToggleId",
                table: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.RenameTable(
                name: "Settings",
                newName: "Setting");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_ToggleId",
                table: "Setting",
                newName: "IX_Setting_ToggleId");

            migrationBuilder.RenameIndex(
                name: "IX_Settings_DialId",
                table: "Setting",
                newName: "IX_Setting_DialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Setting",
                table: "Setting",
                column: "Id");

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
    }
}
