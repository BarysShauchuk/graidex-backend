using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Student_AddColumn_ProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Students");
        }
    }
}
