using Microsoft.EntityFrameworkCore.Migrations;

namespace BarrierLayer.Migrations
{
    public partial class BarrierAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Barriers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Barriers");
        }
    }
}
