using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCelebrations.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnitIdAddToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                table: "Employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UnitId",
                table: "Employees",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Units_UnitId",
                table: "Employees",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Units_UnitId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UnitId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Employees");
        }
    }
}
