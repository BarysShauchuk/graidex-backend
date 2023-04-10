using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeToPass",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_TestId",
                table: "Students",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Tests_TestId",
                table: "Students",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Tests_TestId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_TestId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GradeToPass",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Students");
        }
    }
}
