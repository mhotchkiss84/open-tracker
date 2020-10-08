using Microsoft.EntityFrameworkCore.Migrations;

namespace open_tracker.Migrations
{
    public partial class FixingIssueAssignedMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueAssignedMembers_Issues_IssuesIssueId",
                table: "IssueAssignedMembers");

            migrationBuilder.AlterColumn<int>(
                name: "IssuesIssueId",
                table: "IssueAssignedMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueAssignedMembers_Issues_IssuesIssueId",
                table: "IssueAssignedMembers",
                column: "IssuesIssueId",
                principalTable: "Issues",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueAssignedMembers_Issues_IssuesIssueId",
                table: "IssueAssignedMembers");

            migrationBuilder.AlterColumn<int>(
                name: "IssuesIssueId",
                table: "IssueAssignedMembers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_IssueAssignedMembers_Issues_IssuesIssueId",
                table: "IssueAssignedMembers",
                column: "IssuesIssueId",
                principalTable: "Issues",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
