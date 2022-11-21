using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomGameSelector.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForGameandGenreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Civilization 6"});
            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Risk of Rain 2" });
            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Crusader Kings 3" });
            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Deep Rock Galactic" });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Strategy" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "4X" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "RogueLike" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "First Person Shooter" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Historic" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Role Playing" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 7, "Third Person Shooter" });
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 8, "Action" });

            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 1, 1 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 1, 2 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 2, 3 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 2, 7 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 2, 8 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 3, 5 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 3, 6 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 4, 4 });
            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "GameId", "GenreId" },
                values: new object[] { 4, 8 });
        }
    }
}
