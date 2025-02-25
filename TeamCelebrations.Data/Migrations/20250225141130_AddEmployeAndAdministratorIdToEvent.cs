using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCelebrations.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeAndAdministratorIdToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdministratorId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdministratorId",
                table: "Events",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmployeeId",
                table: "Events",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Employees_AdministratorId",
                table: "Events",
                column: "AdministratorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Employees_EmployeeId",
                table: "Events",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Employees_AdministratorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Employees_EmployeeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AdministratorId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EmployeeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdministratorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Events");
        }
    }
}
