using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumBasic.Data.Migrations
{
    public partial class add_project_controller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 32, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true),
                    ModifierId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_AspNetUsers_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "ProjectID",
                table: "UserStories",
                maxLength:32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_ProjectID",
                table: "UserStories",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatorId",
                table: "Project",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ModifierId",
                table: "Project",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Project_ProjectID",
                table: "UserStories",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Project_ProjectID",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_ProjectID",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
