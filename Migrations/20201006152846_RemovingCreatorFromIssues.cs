using Microsoft.EntityFrameworkCore.Migrations;

namespace open_tracker.Migrations
{
    public partial class RemovingCreatorFromIssues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_AspNetUsers_CreatorId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_CreatorId",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Issues",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Issues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Issues_CreatorId",
                table: "Issues",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_AspNetUsers_CreatorId",
                table: "Issues",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
