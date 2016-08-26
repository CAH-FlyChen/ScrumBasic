using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumBasic.Data.Migrations
{
    public partial class add_creator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignToId",
                table: "UserStories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_AssignToId",
                table: "UserStories",
                column: "AssignToId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_AspNetUsers_AssignToId",
                table: "UserStories",
                column: "AssignToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_AspNetUsers_AssignToId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_AssignToId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "AssignToId",
                table: "UserStories");
        }
    }
}
