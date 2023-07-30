using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToneTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSettings1101231231231244123123123123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AmplifierId",
                table: "Toggles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToneId",
                table: "Pedals",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AmplifierId",
                table: "Dials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Amplifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amplifiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AmplifierId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tones_Amplifiers_AmplifierId",
                        column: x => x.AmplifierId,
                        principalTable: "Amplifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Toggles_AmplifierId",
                table: "Toggles",
                column: "AmplifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedals_ToneId",
                table: "Pedals",
                column: "ToneId");

            migrationBuilder.CreateIndex(
                name: "IX_Dials_AmplifierId",
                table: "Dials",
                column: "AmplifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Tones_AmplifierId",
                table: "Tones",
                column: "AmplifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dials_Amplifiers_AmplifierId",
                table: "Dials",
                column: "AmplifierId",
                principalTable: "Amplifiers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedals_Tones_ToneId",
                table: "Pedals",
                column: "ToneId",
                principalTable: "Tones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Toggles_Amplifiers_AmplifierId",
                table: "Toggles",
                column: "AmplifierId",
                principalTable: "Amplifiers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dials_Amplifiers_AmplifierId",
                table: "Dials");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedals_Tones_ToneId",
                table: "Pedals");

            migrationBuilder.DropForeignKey(
                name: "FK_Toggles_Amplifiers_AmplifierId",
                table: "Toggles");

            migrationBuilder.DropTable(
                name: "Tones");

            migrationBuilder.DropTable(
                name: "Amplifiers");

            migrationBuilder.DropIndex(
                name: "IX_Toggles_AmplifierId",
                table: "Toggles");

            migrationBuilder.DropIndex(
                name: "IX_Pedals_ToneId",
                table: "Pedals");

            migrationBuilder.DropIndex(
                name: "IX_Dials_AmplifierId",
                table: "Dials");

            migrationBuilder.DropColumn(
                name: "AmplifierId",
                table: "Toggles");

            migrationBuilder.DropColumn(
                name: "ToneId",
                table: "Pedals");

            migrationBuilder.DropColumn(
                name: "AmplifierId",
                table: "Dials");
        }
    }
}
