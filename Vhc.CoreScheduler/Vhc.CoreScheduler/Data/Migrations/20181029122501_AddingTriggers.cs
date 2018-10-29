using Microsoft.EntityFrameworkCore.Migrations;

namespace Vhc.CoreScheduler.Data.Migrations
{
    public partial class AddingTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    CronExpression = table.Column<string>(nullable: true),
                    EnvironmentId = table.Column<int>(nullable: true),
                    JobDefinitionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triggers_ExecutionEnvironments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "ExecutionEnvironments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Triggers_Jobs_JobDefinitionId",
                        column: x => x.JobDefinitionId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_EnvironmentId",
                table: "Triggers",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_JobDefinitionId",
                table: "Triggers",
                column: "JobDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Triggers");
        }
    }
}
