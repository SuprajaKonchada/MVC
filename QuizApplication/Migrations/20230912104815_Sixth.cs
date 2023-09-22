using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApplication.Migrations
{
    /// <inheritdoc />
    public partial class Sixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestDuration",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "TestDuration",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
