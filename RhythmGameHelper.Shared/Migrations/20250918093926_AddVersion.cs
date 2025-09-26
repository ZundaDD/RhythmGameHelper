using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "SongInclusions");

            migrationBuilder.AddColumn<int>(
                name: "VersionId",
                table: "SongInclusions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GameVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionName = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateOnly>(type: "date", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameVersions_GameData_GameId",
                        column: x => x.GameId,
                        principalTable: "GameData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongInclusions_VersionId",
                table: "SongInclusions",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVersions_GameId_VersionName",
                table: "GameVersions",
                columns: new[] { "GameId", "VersionName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SongInclusions_GameVersions_VersionId",
                table: "SongInclusions",
                column: "VersionId",
                principalTable: "GameVersions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongInclusions_GameVersions_VersionId",
                table: "SongInclusions");

            migrationBuilder.DropTable(
                name: "GameVersions");

            migrationBuilder.DropIndex(
                name: "IX_SongInclusions_VersionId",
                table: "SongInclusions");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "SongInclusions");

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "SongInclusions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
