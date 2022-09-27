using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDSU_JournalMS.Migrations
{
    public partial class Comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    JournalId = table.Column<int>(type: "int", nullable: false),
                    JournalsId = table.Column<int>(type: "int", nullable: true),
                    CommentBody = table.Column<string>(type: "varchar(2500)", maxLength: 2500, nullable: true),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_EDSUJournals_JournalsId",
                        column: x => x.JournalsId,
                        principalTable: "EDSUJournals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_JournalEditors_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "JournalEditors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_JournalsId",
                table: "Comments",
                column: "JournalsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewerId",
                table: "Comments",
                column: "ReviewerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
