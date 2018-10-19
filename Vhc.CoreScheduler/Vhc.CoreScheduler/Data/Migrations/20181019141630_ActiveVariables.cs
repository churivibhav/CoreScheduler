using Microsoft.EntityFrameworkCore.Migrations;

namespace Vhc.CoreScheduler.Data.Migrations
{
    public partial class ActiveVariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Variables",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Variables");
        }
    }
}
