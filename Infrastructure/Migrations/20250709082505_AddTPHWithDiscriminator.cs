using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTPHWithDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          //  migrationBuilder.DropTable(
             //   name: "EmergencyReports");

           // migrationBuilder.DropTable(
               // name: "SocialMediaReports");

            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "IssueReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "IssueReports",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IssueReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmergencyServiceId",
                table: "IssueReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "IssueReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostUrl",
                table: "IssueReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shares",
                table: "IssueReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssueReports_EmergencyServiceId",
                table: "IssueReports",
                column: "EmergencyServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueReports_EmergencyServices_EmergencyServiceId",
                table: "IssueReports",
                column: "EmergencyServiceId",
                principalTable: "EmergencyServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueReports_EmergencyServices_EmergencyServiceId",
                table: "IssueReports");

            migrationBuilder.DropIndex(
                name: "IX_IssueReports_EmergencyServiceId",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "EmergencyServiceId",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "PostUrl",
                table: "IssueReports");

            migrationBuilder.DropColumn(
                name: "Shares",
                table: "IssueReports");

            migrationBuilder.CreateTable(
                name: "EmergencyReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmergencyServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyReports_EmergencyServices_EmergencyServiceId",
                        column: x => x.EmergencyServiceId,
                        principalTable: "EmergencyServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmergencyReports_IssueReports_Id",
                        column: x => x.Id,
                        principalTable: "IssueReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    PostUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shares = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialMediaReports_IssueReports_Id",
                        column: x => x.Id,
                        principalTable: "IssueReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyReports_EmergencyServiceId",
                table: "EmergencyReports",
                column: "EmergencyServiceId");
        }
    }
}
