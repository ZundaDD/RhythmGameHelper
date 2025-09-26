using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SongName",
                table: "SongData",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "VersionName",
                table: "GameVersions",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_GameVersions_GameId_VersionName",
                table: "GameVersions",
                newName: "IX_GameVersions_GameId_Name");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "GameCategories",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategories_GameId_CategoryName",
                table: "GameCategories",
                newName: "IX_GameCategories_GameId_Name");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chart_SongInclusions_SongInclusionSongId_SongInclusionGameId",
                table: "Chart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chart",
                table: "Chart");

            migrationBuilder.RenameTable(
                name: "Chart",
                newName: "ChartData");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SongData",
                newName: "SongName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GameVersions",
                newName: "VersionName");

            migrationBuilder.RenameIndex(
                name: "IX_GameVersions_GameId_Name",
                table: "GameVersions",
                newName: "IX_GameVersions_GameId_VersionName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GameCategories",
                newName: "CategoryName");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategories_GameId_Name",
                table: "GameCategories",
                newName: "IX_GameCategories_GameId_CategoryName");

            migrationBuilder.RenameIndex(
                name: "IX_Chart_SongInclusionSongId_SongInclusionGameId",
                table: "ChartData",
                newName: "IX_ChartData_SongInclusionSongId_SongInclusionGameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChartData",
                table: "ChartData",
                column: "Id");

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
