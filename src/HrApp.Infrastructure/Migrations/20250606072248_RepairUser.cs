using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RepairUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Team_TeamLeaderId",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Team_TeamLeaderId",
                table: "User",
                column: "TeamLeaderId",
                principalTable: "Team",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Team_TeamLeaderId",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Team_TeamLeaderId",
                table: "User",
                column: "TeamLeaderId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
