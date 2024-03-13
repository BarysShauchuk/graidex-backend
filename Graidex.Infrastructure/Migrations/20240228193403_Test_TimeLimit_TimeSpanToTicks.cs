using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Test_TimeLimit_TimeSpanToTicks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeLimit_Tmp",
                table: "Tests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.Sql(
                """
                UPDATE Tests 
                SET TimeLimit_Tmp = 
                    (CAST(DATEDIFF(SECOND, '00:00:00', TimeLimit) AS bigint) * 10000000)
                """);

            migrationBuilder.DropColumn(
                name: "TimeLimit", 
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "TimeLimit_Tmp", 
                table: "Tests",
                newName: "TimeLimit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeLimit_Tmp",
                table: "Tests",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.Sql(
                """
                UPDATE Tests
                SET TimeLimit_Tmp = 
                    CAST(DATEADD(SECOND, (TimeLimit / 10000000) % 86400, 0) AS TIME)
                """);

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "TimeLimit_Tmp",
                table: "Tests",
                newName: "TimeLimit");
        }
    }
}
