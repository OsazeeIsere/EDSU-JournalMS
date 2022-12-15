using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDSU_JournalMS.Migrations
{
    public partial class moreReviewers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EighthReviewer",
                table: "EDSUJournals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NinthReviewer",
                table: "EDSUJournals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeventhReviewer",
                table: "EDSUJournals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SixthReviewer",
                table: "EDSUJournals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenthReviewer",
                table: "EDSUJournals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EighthReviewer",
                table: "EDSUJournals");

            migrationBuilder.DropColumn(
                name: "NinthReviewer",
                table: "EDSUJournals");

            migrationBuilder.DropColumn(
                name: "SeventhReviewer",
                table: "EDSUJournals");

            migrationBuilder.DropColumn(
                name: "SixthReviewer",
                table: "EDSUJournals");

            migrationBuilder.DropColumn(
                name: "TenthReviewer",
                table: "EDSUJournals");
        }
    }
}
