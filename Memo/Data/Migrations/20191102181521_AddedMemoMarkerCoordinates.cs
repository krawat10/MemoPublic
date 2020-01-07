using Microsoft.EntityFrameworkCore.Migrations;

namespace Memo.Migrations
{
    public partial class AddedMemoMarkerCoordinates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Coordinates_Latitude",
                table: "MemoMarkers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Coordinates_Longitude",
                table: "MemoMarkers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates_Latitude",
                table: "MemoMarkers");

            migrationBuilder.DropColumn(
                name: "Coordinates_Longitude",
                table: "MemoMarkers");
        }
    }
}
