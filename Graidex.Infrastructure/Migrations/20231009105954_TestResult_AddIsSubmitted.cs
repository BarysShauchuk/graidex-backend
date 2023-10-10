using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestResult_AddIsSubmitted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "TestResults",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "TestResults");
        }
    }
}
