using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class p2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamRaport_TeamLeaderId",
                table: "TeamRaport");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRaport_TeamLeaderId",
                table: "TeamRaport",
                column: "TeamLeaderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamRaport_TeamLeaderId",
                table: "TeamRaport");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRaport_TeamLeaderId",
                table: "TeamRaport",
                column: "TeamLeaderId");
        }
    }
}
