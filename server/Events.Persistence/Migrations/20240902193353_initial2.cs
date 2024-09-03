using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventRegistrationDate",
                table: "Participant");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventRegistrationDate",
                table: "EventParticipant",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventRegistrationDate",
                table: "EventParticipant");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventRegistrationDate",
                table: "Participant",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
