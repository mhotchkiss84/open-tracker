using Microsoft.EntityFrameworkCore.Migrations;

namespace open_tracker.Migrations
{
    public partial class AddedRequiredBackToProjectMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectMembers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_AspNetUsers_UserId",
                table: "ProjectMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
