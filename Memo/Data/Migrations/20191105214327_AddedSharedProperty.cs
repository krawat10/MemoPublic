using Microsoft.EntityFrameworkCore.Migrations;

namespace Memo.Migrations
{
    public partial class AddedSharedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Shared",
                table: "MemoMarkers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shared",
                table: "MemoMarkers");
        }
    }
}
