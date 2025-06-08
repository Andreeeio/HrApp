using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class repiardb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentRaport_TeamRaport_AssignedToTeamId",
                table: "AssignmentRaport");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamRaport_UserRaport_TeamLeaderId",
                table: "TeamRaport");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRaport_TeamRaport_TeamId",
                table: "UserRaport");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRaport_TeamRaport_TeamLeaderId",
                table: "UserRaport");

            migrationBuilder.DropIndex(
                name: "IX_UserRaport_TeamId",
                table: "UserRaport");

            migrationBuilder.DropIndex(
                name: "IX_UserRaport_TeamLeaderId",
                table: "UserRaport");

            migrationBuilder.DropIndex(
                name: "IX_TeamRaport_TeamLeaderId",
                table: "TeamRaport");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentRaport_AssignedToTeamId",
                table: "AssignmentRaport");

            migrationBuilder.DropColumn(
                name: "TeamLeaderId",
                table: "UserRaport");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AssignmentRaport",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AssignmentRaport",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamLeaderId",
                table: "UserRaport",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AssignmentRaport",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AssignmentRaport",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserRaport_TeamId",
                table: "UserRaport",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRaport_TeamLeaderId",
                table: "UserRaport",
                column: "TeamLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRaport_TeamLeaderId",
                table: "TeamRaport",
                column: "TeamLeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentRaport_AssignedToTeamId",
                table: "AssignmentRaport",
                column: "AssignedToTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentRaport_TeamRaport_AssignedToTeamId",
                table: "AssignmentRaport",
                column: "AssignedToTeamId",
                principalTable: "TeamRaport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamRaport_UserRaport_TeamLeaderId",
                table: "TeamRaport",
                column: "TeamLeaderId",
                principalTable: "UserRaport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRaport_TeamRaport_TeamId",
                table: "UserRaport",
                column: "TeamId",
                principalTable: "TeamRaport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRaport_TeamRaport_TeamLeaderId",
                table: "UserRaport",
                column: "TeamLeaderId",
                principalTable: "TeamRaport",
                principalColumn: "Id");
        }
    }
}
