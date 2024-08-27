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
            migrationBuilder.DropTable(
                name: "EventEntityParticipantEntity");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Participant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipantEntity_ParticipantId",
                table: "EventParticipantEntity",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipantEntity_Event_EventId",
                table: "EventParticipantEntity",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipantEntity_Participant_ParticipantId",
                table: "EventParticipantEntity",
                column: "ParticipantId",
                principalTable: "Participant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipantEntity_Event_EventId",
                table: "EventParticipantEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipantEntity_Participant_ParticipantId",
                table: "EventParticipantEntity");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipantEntity_ParticipantId",
                table: "EventParticipantEntity");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Participant");

            migrationBuilder.CreateTable(
                name: "EventEntityParticipantEntity",
                columns: table => new
                {
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEntityParticipantEntity", x => new { x.EventsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_EventEntityParticipantEntity_Event_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEntityParticipantEntity_Participant_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEntityParticipantEntity_ParticipantsId",
                table: "EventEntityParticipantEntity",
                column: "ParticipantsId");
        }
    }
}
