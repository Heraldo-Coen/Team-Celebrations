using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamCelebrations.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCostraintsToEmployeeIDsToFriendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friendships_EmployeeId1",
                table: "Friendships");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_EmployeeId1_EmployeeId2",
                table: "Friendships",
                columns: new[] { "EmployeeId1", "EmployeeId2" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friendships_EmployeeId1_EmployeeId2",
                table: "Friendships");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_EmployeeId1",
                table: "Friendships",
                column: "EmployeeId1");
        }
    }
}
