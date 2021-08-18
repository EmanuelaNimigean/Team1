using Microsoft.EntityFrameworkCore.Migrations;

namespace Team1Project.Data.Migrations
{
    public partial class Intern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubUsername",
                table: "Intern",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubUsername",
                table: "Intern");
        }
    }
}
