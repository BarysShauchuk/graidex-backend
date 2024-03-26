using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Tests_Questions_TestResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewResult",
                table: "Tests",
                newName: "ShowToStudent");

            migrationBuilder.RenameColumn(
                name: "IsAutoChecked",
                table: "TestResults",
                newName: "ShowToStudent");

            migrationBuilder.RenameColumn(
                name: "CanReview",
                table: "TestResults",
                newName: "RequireTeacherReview");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowToStudent",
                table: "Tests",
                newName: "ReviewResult");

            migrationBuilder.RenameColumn(
                name: "ShowToStudent",
                table: "TestResults",
                newName: "IsAutoChecked");

            migrationBuilder.RenameColumn(
                name: "RequireTeacherReview",
                table: "TestResults",
                newName: "CanReview");
        }
    }
}
