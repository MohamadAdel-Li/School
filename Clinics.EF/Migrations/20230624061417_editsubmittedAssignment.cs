using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinics.EF.Migrations
{
    /// <inheritdoc />
    public partial class editsubmittedAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "SubmittedAssignments",
                newName: "description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "SubmittedAssignments",
                newName: "name");
        }
    }
}
