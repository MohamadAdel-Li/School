using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinics.EF.Migrations
{
    /// <inheritdoc />
    public partial class marks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mark",
                table: "SubmittedAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mark",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "SubmittedAssignments");

            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Assignments");
        }
    }
}
