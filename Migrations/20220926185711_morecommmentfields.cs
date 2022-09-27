using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDSU_JournalMS.Migrations
{
    public partial class morecommmentfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerNo",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReviewerNo",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Comments");
        }
    }
}
