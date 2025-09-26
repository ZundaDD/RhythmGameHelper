using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
