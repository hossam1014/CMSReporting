using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameInEmergency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "EmergencyServices",
                newName: "NameEN");

            migrationBuilder.AddColumn<string>(
                name: "NameAR",
                table: "EmergencyServices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAR",
                table: "EmergencyServices");

            migrationBuilder.RenameColumn(
                name: "NameEN",
                table: "EmergencyServices",
                newName: "Name");
        }
    }
}
