using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitTestToTestBaseAndTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTest_Students_AllowedStudentsId",
                table: "StudentTest");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "IsHidden",
                table: "Tests",
                newName: "IsVisible");

            migrationBuilder.RenameColumn(
                name: "AllowedStudentsId",
                table: "StudentTest",
                newName: "RestrictionGroupId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeLimit",
                table: "Tests",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "Tests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestrictionRule",
                table: "Tests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "Tests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTest_Students_RestrictionGroupId",
                table: "StudentTest",
                column: "RestrictionGroupId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTest_Students_RestrictionGroupId",
                table: "StudentTest");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "RestrictionRule",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "Tests");

            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "Tests",
                newName: "IsHidden");

            migrationBuilder.RenameColumn(
                name: "RestrictionGroupId",
                table: "StudentTest",
                newName: "AllowedStudentsId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TimeLimit",
                table: "Tests",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTest_Students_AllowedStudentsId",
                table: "StudentTest",
                column: "AllowedStudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
