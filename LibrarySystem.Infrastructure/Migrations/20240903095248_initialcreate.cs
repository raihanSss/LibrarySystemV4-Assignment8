using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "process",
                columns: table => new
                {
                    id_process = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    processname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("process_pkey", x => x.id_process);
                });

            migrationBuilder.CreateTable(
                name: "workflow",
                columns: table => new
                {
                    id_workflow = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    workflowname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("workflow_pkey", x => x.id_workflow);
                });

            migrationBuilder.CreateTable(
                name: "request",
                columns: table => new
                {
                    id_request = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_workflow = table.Column<int>(type: "integer", nullable: false),
                    id_requester = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    requesttype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    currentstepid = table.Column<int>(type: "integer", nullable: false),
                    requestdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    id_process = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("request_pkey", x => x.id_request);
                    table.ForeignKey(
                        name: "request_id_process_fkey",
                        column: x => x.id_process,
                        principalTable: "process",
                        principalColumn: "id_process",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "request_id_workflow_fkey",
                        column: x => x.id_workflow,
                        principalTable: "workflow",
                        principalColumn: "id_workflow",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_sequence",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_workflow = table.Column<int>(type: "integer", nullable: false),
                    steporder = table.Column<int>(type: "integer", nullable: false),
                    stepname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    requiredrole = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("workflow_sequence_pkey", x => x.id);
                    table.ForeignKey(
                        name: "workflow_sequence_id_workflow_fkey",
                        column: x => x.id_workflow,
                        principalTable: "workflow",
                        principalColumn: "id_workflow",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_action",
                columns: table => new
                {
                    id_action = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_request = table.Column<int>(type: "integer", nullable: false),
                    id_step = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    actiondate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    comments = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("workflow_action_pkey", x => x.id_action);
                    table.ForeignKey(
                        name: "workflow_action_id_request_fkey",
                        column: x => x.id_request,
                        principalTable: "request",
                        principalColumn: "id_request",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "workflow_action_id_step_fkey",
                        column: x => x.id_step,
                        principalTable: "workflow_sequence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_request_id_process",
                table: "request",
                column: "id_process");

            migrationBuilder.CreateIndex(
                name: "IX_request_id_workflow",
                table: "request",
                column: "id_workflow");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_action_id_request",
                table: "workflow_action",
                column: "id_request");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_action_id_step",
                table: "workflow_action",
                column: "id_step");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_sequence_id_workflow",
                table: "workflow_sequence",
                column: "id_workflow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workflow_action");

            migrationBuilder.DropTable(
                name: "request");

            migrationBuilder.DropTable(
                name: "workflow_sequence");

            migrationBuilder.DropTable(
                name: "process");

            migrationBuilder.DropTable(
                name: "workflow");
        }
    }
}
