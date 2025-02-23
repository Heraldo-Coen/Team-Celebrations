// Ignore Spelling: Dni

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCelebrations.Data.Migrations
{
    /// <inheritdoc />
    public partial class DniAndVerifiedPersonAddToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Units_HigherUnitId",
                table: "Units");

            migrationBuilder.AlterColumn<Guid>(
                name: "HigherUnitId",
                table: "Units",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPhoneVerified",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DNI",
                table: "Employees",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DNI",
                table: "Employees",
                column: "DNI",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Units_HigherUnitId",
                table: "Units",
                column: "HigherUnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Units_HigherUnitId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DNI",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DNI",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "HigherUnitId",
                table: "Units",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPhoneVerified",
                table: "Employees",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Units_HigherUnitId",
                table: "Units",
                column: "HigherUnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
