using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxPointsToTestBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPoints",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPoints",
                table: "TestDrafts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "TestDrafts");
        }
    }
}
