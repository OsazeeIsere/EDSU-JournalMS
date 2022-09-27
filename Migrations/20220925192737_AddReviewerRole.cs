using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDSU_JournalMS.Migrations
{
    public partial class AddReviewerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "JournalEditors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "JournalEditors");
        }
    }
}
