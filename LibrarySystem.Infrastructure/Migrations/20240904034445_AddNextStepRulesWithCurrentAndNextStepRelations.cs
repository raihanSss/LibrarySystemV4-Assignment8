using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNextStepRulesWithCurrentAndNextStepRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nextstep",
                table: "workflow_sequence");

            migrationBuilder.CreateTable(
                name: "next_step_rule",
                columns: table => new
                {
                    id_rule = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_currentstep = table.Column<int>(type: "integer", nullable: false),
                    id_nextstep = table.Column<int>(type: "integer", nullable: false),
                    conditiontype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    conditionvalue = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("next_step_rule_pkey", x => x.id_rule);
                    table.ForeignKey(
                        name: "next_step_rule_id_currentstep_fkey",
                        column: x => x.id_currentstep,
                        principalTable: "workflow_sequence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "next_step_rule_id_nextstep_fkey",
                        column: x => x.id_nextstep,
                        principalTable: "workflow_sequence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_next_step_rule_id_currentstep",
                table: "next_step_rule",
                column: "id_currentstep");

            migrationBuilder.CreateIndex(
                name: "IX_next_step_rule_id_nextstep",
                table: "next_step_rule",
                column: "id_nextstep");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "next_step_rule");

            migrationBuilder.AddColumn<int>(
                name: "nextstep",
                table: "workflow_sequence",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
