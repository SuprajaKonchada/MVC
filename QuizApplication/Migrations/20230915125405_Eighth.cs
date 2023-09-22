using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApplication.Migrations
{
    /// <inheritdoc />
    public partial class Eighth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StdPassword",
                table: "Students",
                newName: "StudentPassword");

            migrationBuilder.RenameColumn(
                name: "StdName",
                table: "Students",
                newName: "StudentName");

            migrationBuilder.RenameColumn(
                name: "StdEmail",
                table: "Students",
                newName: "StudentEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentPassword",
                table: "Students",
                newName: "StdPassword");

            migrationBuilder.RenameColumn(
                name: "StudentName",
                table: "Students",
                newName: "StdName");

            migrationBuilder.RenameColumn(
                name: "StudentEmail",
                table: "Students",
                newName: "StdEmail");
        }
    }
}
