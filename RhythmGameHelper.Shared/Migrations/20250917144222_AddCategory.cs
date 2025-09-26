using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RhythmGameHelper.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartData_SongInclusions_SongId_GameId",
                table: "ChartData");

            migrationBuilder.DropIndex(
                name: "IX_ChartData_SongId_GameId",
                table: "ChartData");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "SongInclusions",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "GameCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCategories_GameData_GameId",
                        column: x => x.GameId,
                        principalTable: "GameData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongInclusions_CategoryId",
                table: "SongInclusions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartData_SongInclusionSongId_SongInclusionGameId",
                table: "ChartData",
                columns: new[] { "SongInclusionSongId", "SongInclusionGameId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameCategories_GameId_CategoryName",
                table: "GameCategories",
                columns: new[] { "GameId", "CategoryName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartData_SongInclusions_SongInclusionSongId_SongInclusionG~",
                table: "ChartData",
                columns: new[] { "SongInclusionSongId", "SongInclusionGameId" },
                principalTable: "SongInclusions",
                principalColumns: new[] { "SongId", "GameId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongInclusions_GameCategories_CategoryId",
                table: "SongInclusions",
                column: "CategoryId",
                principalTable: "GameCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartData_SongInclusions_SongInclusionSongId_SongInclusionG~",
                table: "ChartData");

            migrationBuilder.DropForeignKey(
                name: "FK_SongInclusions_GameCategories_CategoryId",
                table: "SongInclusions");

            migrationBuilder.DropTable(
                name: "GameCategories");

            migrationBuilder.DropIndex(
                name: "IX_SongInclusions_CategoryId",
                table: "SongInclusions");

            migrationBuilder.DropIndex(
                name: "IX_ChartData_SongInclusionSongId_SongInclusionGameId",
                table: "ChartData");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SongInclusions");

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
    }
}
