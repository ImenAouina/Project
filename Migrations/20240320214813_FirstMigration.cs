using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chore_tracker.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_CreatorUserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CreatorUserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_UserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "CreatorUserId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CreatorUserId",
                table: "Jobs",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_CreatorUserId",
                table: "Jobs",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
