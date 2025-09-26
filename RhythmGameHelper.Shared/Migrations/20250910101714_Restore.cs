using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class Restore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartData_SongInclusions_SongInclusionSongId_SongInclusionG~",
                table: "ChartData");

            migrationBuilder.DropIndex(
                name: "IX_ChartData_SongInclusionSongId_SongInclusionGameId",
                table: "ChartData");

            migrationBuilder.DropColumn(
                name: "SongInclusionGameId",
                table: "ChartData");

            migrationBuilder.DropColumn(
                name: "SongInclusionSongId",
                table: "ChartData");

            migrationBuilder.CreateIndex(
                name: "IX_ChartData_SongId_GameId",
                table: "ChartData",
                columns: new[] { "SongId", "GameId" });

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

            migrationBuilder.DropIndex(
                name: "IX_ChartData_SongId_GameId",
                table: "ChartData");

            migrationBuilder.AddColumn<int>(
                name: "SongInclusionGameId",
                table: "ChartData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SongInclusionSongId",
                table: "ChartData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChartData_SongInclusionSongId_SongInclusionGameId",
                table: "ChartData",
                columns: new[] { "SongInclusionSongId", "SongInclusionGameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChartData_SongInclusions_SongInclusionSongId_SongInclusionG~",
                table: "ChartData",
                columns: new[] { "SongInclusionSongId", "SongInclusionGameId" },
                principalTable: "SongInclusions",
                principalColumns: new[] { "SongId", "GameId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
