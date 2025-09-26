using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class NewDataStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "SongData",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "MaxOfficialBPM",
                table: "SongData",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "OfficialBPM",
                table: "SongData",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<List<string>>(
                name: "Version",
                table: "SongData",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delete",
                table: "SongData");

            migrationBuilder.DropColumn(
                name: "MaxOfficialBPM",
                table: "SongData");

            migrationBuilder.DropColumn(
                name: "OfficialBPM",
                table: "SongData");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "SongData");
        }
    }
}
