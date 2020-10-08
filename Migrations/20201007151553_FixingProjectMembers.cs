using Microsoft.EntityFrameworkCore.Migrations;

namespace open_tracker.Migrations
{
    public partial class FixingProjectMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectsProjectId",
                table: "ProjectMembers");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectsProjectId",
                table: "ProjectMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectsProjectId",
                table: "ProjectMembers",
                column: "ProjectsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectsProjectId",
                table: "ProjectMembers");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectsProjectId",
                table: "ProjectMembers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_Projects_ProjectsProjectId",
                table: "ProjectMembers",
                column: "ProjectsProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
