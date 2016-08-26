using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumBasic.Data.Migrations
{
    public partial class add_story_creator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "UserStories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_CreatorId",
                table: "UserStories",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_AspNetUsers_CreatorId",
                table: "UserStories",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_AspNetUsers_CreatorId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_CreatorId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "UserStories");
        }
    }
}
