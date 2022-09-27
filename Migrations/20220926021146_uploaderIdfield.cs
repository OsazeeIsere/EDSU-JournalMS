using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDSU_JournalMS.Migrations
{
    public partial class uploaderIdfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploaderId",
                table: "EDSUJournals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploaderId",
                table: "EDSUJournals");
        }
    }
}
