using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Participant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Admin",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Admin");
        }
    }
}
