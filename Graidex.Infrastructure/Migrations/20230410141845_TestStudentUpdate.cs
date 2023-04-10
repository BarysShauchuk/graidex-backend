using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestStudentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Tests_TestId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_TestId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "StudentTest",
                columns: table => new
                {
                    AllowedStudentsId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTest", x => new { x.AllowedStudentsId, x.TestId });
                    table.ForeignKey(
                        name: "FK_StudentTest_Students_AllowedStudentsId",
                        column: x => x.AllowedStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTest_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTest_TestId",
                table: "StudentTest",
                column: "TestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTest");

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
    }
}
