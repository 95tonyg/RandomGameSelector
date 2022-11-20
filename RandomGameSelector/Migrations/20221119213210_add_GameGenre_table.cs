using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomGameSelector.Migrations
{
    /// <inheritdoc />
    public partial class addGameGenretable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.GameId, x.GenreId });
                    table.ForeignKey("FK_Game_GameGenre", x => x.GameId, "Game");
                    table.ForeignKey("FK_Genre_GameGenre", x => x.GenreId, "Genre");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenre");
        }
    }
}
