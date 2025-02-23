using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCelebrations.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeAndFriendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_PhoneCodeId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptanceDate",
                table: "Friendships",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Friendships",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Employees",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneCodeId_PhoneNumber",
                table: "Employees",
                columns: new[] { "PhoneCodeId", "PhoneNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_PhoneCodeId_PhoneNumber",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AcceptanceDate",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Friendships");

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Employees",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneCodeId",
                table: "Employees",
                column: "PhoneCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Employees",
                column: "PhoneNumber",
                unique: true);
        }
    }
}
