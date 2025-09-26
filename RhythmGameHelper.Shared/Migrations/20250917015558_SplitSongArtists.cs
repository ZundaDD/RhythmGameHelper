using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class SplitSongArtists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Artist",
                table: "SongData",
                newName: "OfficialArtist");

            migrationBuilder.AddColumn<List<string>>(
                name: "Artists",
                table: "SongData",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artists",
                table: "SongData");

            migrationBuilder.RenameColumn(
                name: "OfficialArtist",
                table: "SongData",
                newName: "Artist");
        }
    }
}
