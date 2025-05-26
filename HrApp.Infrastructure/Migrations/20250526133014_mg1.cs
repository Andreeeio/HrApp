using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkLogExportHistory_ExportedByUserId",
                table: "WorkLogExportHistory",
                column: "ExportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogExportHistory_ExportedForUserId",
                table: "WorkLogExportHistory",
                column: "ExportedForUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogExportHistory_User_ExportedByUserId",
                table: "WorkLogExportHistory",
                column: "ExportedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogExportHistory_User_ExportedForUserId",
                table: "WorkLogExportHistory",
                column: "ExportedForUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogExportHistory_User_ExportedByUserId",
                table: "WorkLogExportHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogExportHistory_User_ExportedForUserId",
                table: "WorkLogExportHistory");

            migrationBuilder.DropIndex(
                name: "IX_WorkLogExportHistory_ExportedByUserId",
                table: "WorkLogExportHistory");

            migrationBuilder.DropIndex(
                name: "IX_WorkLogExportHistory_ExportedForUserId",
                table: "WorkLogExportHistory");
        }
    }
}
