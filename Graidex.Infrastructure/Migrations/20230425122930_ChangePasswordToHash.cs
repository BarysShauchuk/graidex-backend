﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graidex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePasswordToHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Teachers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Students",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Teachers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Students",
                newName: "Password");
        }
    }
}
