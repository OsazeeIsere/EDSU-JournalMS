using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDSU_JournalMS.Migrations
{
    public partial class UpdateComentmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_JournalEditors_ReviewerId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReviewerId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReviewerId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "JournalEditorsId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewerEmail",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_JournalEditorsId",
                table: "Comments",
                column: "JournalEditorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_JournalEditors_JournalEditorsId",
                table: "Comments",
                column: "JournalEditorsId",
                principalTable: "JournalEditors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_JournalEditors_JournalEditorsId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_JournalEditorsId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "JournalEditorsId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReviewerEmail",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "ReviewerId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewerId",
                table: "Comments",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_JournalEditors_ReviewerId",
                table: "Comments",
                column: "ReviewerId",
                principalTable: "JournalEditors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
