using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class BreakAndNewStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartData_GameData_GameId",
                table: "ChartData");

            migrationBuilder.DropForeignKey(
                name: "FK_ChartData_SongData_SongId",
                table: "ChartData");

            migrationBuilder.DropIndex(
                name: "IX_ChartData_GameId",
                table: "ChartData");

            migrationBuilder.DropIndex(
                name: "IX_ChartData_SongId",
                table: "ChartData");

            migrationBuilder.DropColumn(
                name: "CollaborationGame",
                table: "SongData");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "SongData");

            migrationBuilder.DropColumn(
                name: "OriginGame",
                table: "SongData");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "SongData");

            migrationBuilder.CreateTable(
                name: "SongInclusions",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    SongId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    Collaboration = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongInclusions", x => new { x.SongId, x.GameId });
                    table.ForeignKey(
                        name: "FK_SongInclusions_GameData_GameId",
                        column: x => x.GameId,
                        principalTable: "GameData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongInclusions_SongData_SongId",
                        column: x => x.SongId,
                        principalTable: "SongData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartData_SongId_GameId",
                table: "ChartData",
                columns: new[] { "SongId", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_SongInclusions_GameId",
                table: "SongInclusions",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartData_SongInclusions_SongId_GameId",
                table: "ChartData",
                columns: new[] { "SongId", "GameId" },
                principalTable: "SongInclusions",
                principalColumns: new[] { "SongId", "GameId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartData_SongInclusions_SongId_GameId",
                table: "ChartData");

            migrationBuilder.DropTable(
                name: "SongInclusions");

            migrationBuilder.DropIndex(
                name: "IX_ChartData_SongId_GameId",
                table: "ChartData");

            migrationBuilder.AddColumn<List<string>>(
                name: "CollaborationGame",
                table: "SongData",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "SongData",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "OriginGame",
                table: "SongData",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "Version",
                table: "SongData",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_ChartData_GameId",
                table: "ChartData",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartData_SongId",
                table: "ChartData",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartData_GameData_GameId",
                table: "ChartData",
                column: "GameId",
                principalTable: "GameData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartData_SongData_SongId",
                table: "ChartData",
                column: "SongId",
                principalTable: "SongData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
