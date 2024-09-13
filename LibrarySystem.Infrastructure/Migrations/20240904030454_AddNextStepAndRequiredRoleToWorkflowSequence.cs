using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNextStepAndRequiredRoleToWorkflowSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "requiredrole",
                table: "workflow_sequence",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "nextstep",
                table: "workflow_sequence",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_workflow_sequence_requiredrole",
                table: "workflow_sequence",
                column: "requiredrole");

            migrationBuilder.AddForeignKey(
                name: "workflow_sequence_requiredrole_fkey",
                table: "workflow_sequence",
                column: "requiredrole",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "workflow_sequence_requiredrole_fkey",
                table: "workflow_sequence");

            migrationBuilder.DropIndex(
                name: "IX_workflow_sequence_requiredrole",
                table: "workflow_sequence");

            migrationBuilder.DropColumn(
                name: "nextstep",
                table: "workflow_sequence");

            migrationBuilder.AlterColumn<string>(
                name: "requiredrole",
                table: "workflow_sequence",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
