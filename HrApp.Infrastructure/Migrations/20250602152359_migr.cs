using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkedHours",
                table: "UserRaport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkedHours",
                table: "UserRaport",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
