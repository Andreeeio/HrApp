using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class boolAsig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "Assignment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "Assignment");
        }
    }
}
