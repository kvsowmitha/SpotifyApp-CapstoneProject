using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbumService.Migrations
{
    public partial class CMdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    MusicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MusicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SingerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SongUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.MusicId);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    MusicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MusicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SingerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SongUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.MusicId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
