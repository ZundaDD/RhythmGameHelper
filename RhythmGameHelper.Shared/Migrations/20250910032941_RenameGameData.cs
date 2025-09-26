using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameGameData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameData",
                table: "GameData");

            migrationBuilder.RenameTable(
                name: "GameData",
                newName: "SongData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongData",
                table: "SongData",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongData",
                table: "SongData");

            migrationBuilder.RenameTable(
                name: "SongData",
                newName: "GameData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameData",
                table: "GameData",
                column: "Id");
        }
    }
}
