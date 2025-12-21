using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FutureDomainFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Besokt",
                table: "ProfilBesok",
                newName: "VisitedUtc");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjektAnvandare",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConnectedUtc",
                table: "ProjektAnvandare",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Projekt",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedUtc",
                table: "Projekt",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedUtc",
                table: "Profiler",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Profiler",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "Profiler",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedUtc",
                table: "Profiler",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "ProfilBesok",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisitorUserId",
                table: "ProfilBesok",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjektAnvandare_ProjectId_UserId",
                table: "ProjektAnvandare",
                columns: new[] { "ProjectId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projekt_CreatedByUserId",
                table: "Projekt",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekt_CreatedUtc",
                table: "Projekt",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Profiler_CreatedUtc",
                table: "Profiler",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Profiler_IsPublic",
                table: "Profiler",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_Profiler_OwnerUserId",
                table: "Profiler",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilBesok_ProfileId",
                table: "ProfilBesok",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilBesok_VisitedUtc",
                table: "ProfilBesok",
                column: "VisitedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_ConversationId",
                table: "DirectMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_SentUtc",
                table: "DirectMessages",
                column: "SentUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjektAnvandare_ProjectId_UserId",
                table: "ProjektAnvandare");

            migrationBuilder.DropIndex(
                name: "IX_Projekt_CreatedByUserId",
                table: "Projekt");

            migrationBuilder.DropIndex(
                name: "IX_Projekt_CreatedUtc",
                table: "Projekt");

            migrationBuilder.DropIndex(
                name: "IX_Profiler_CreatedUtc",
                table: "Profiler");

            migrationBuilder.DropIndex(
                name: "IX_Profiler_IsPublic",
                table: "Profiler");

            migrationBuilder.DropIndex(
                name: "IX_Profiler_OwnerUserId",
                table: "Profiler");

            migrationBuilder.DropIndex(
                name: "IX_ProfilBesok_ProfileId",
                table: "ProfilBesok");

            migrationBuilder.DropIndex(
                name: "IX_ProfilBesok_VisitedUtc",
                table: "ProfilBesok");

            migrationBuilder.DropIndex(
                name: "IX_DirectMessages_ConversationId",
                table: "DirectMessages");

            migrationBuilder.DropIndex(
                name: "IX_DirectMessages_SentUtc",
                table: "DirectMessages");

            migrationBuilder.DropColumn(
                name: "ConnectedUtc",
                table: "ProjektAnvandare");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Projekt");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Projekt");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Profiler");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Profiler");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Profiler");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Profiler");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "ProfilBesok");

            migrationBuilder.DropColumn(
                name: "VisitorUserId",
                table: "ProfilBesok");

            migrationBuilder.RenameColumn(
                name: "VisitedUtc",
                table: "ProfilBesok",
                newName: "Besokt");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjektAnvandare",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
