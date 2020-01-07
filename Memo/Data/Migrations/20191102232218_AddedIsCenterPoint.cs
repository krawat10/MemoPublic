using Microsoft.EntityFrameworkCore.Migrations;

namespace Memo.Migrations
{
    public partial class AddedIsCenterPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCenterPoint",
                table: "MemoMarkers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCenterPoint",
                table: "MemoMarkers");
        }
    }
}
