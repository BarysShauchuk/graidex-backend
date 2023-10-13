using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Test_TestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "TestResults");

            migrationBuilder.AddColumn<bool>(
                name: "CanReview",
                table: "TestResults",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoCheckAfterSubmission",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReviewResult",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "TestResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "TestResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoCheckAfterSubmission",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ReviewResult",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "CanReview",
                table: "TestResults");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "TestResults",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
