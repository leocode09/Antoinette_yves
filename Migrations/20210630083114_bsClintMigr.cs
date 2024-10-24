using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeCareWebApp.Migrations
{
    public partial class bsClintMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "Bzn",
                table: "BusinessClient",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessClient_UserId",
                schema: "Bzn",
                table: "BusinessClient",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessClient_User_UserId",
                schema: "Bzn",
                table: "BusinessClient",
                column: "UserId",
                principalSchema: "Security",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessClient_User_UserId",
                schema: "Bzn",
                table: "BusinessClient");

            migrationBuilder.DropIndex(
                name: "IX_BusinessClient_UserId",
                schema: "Bzn",
                table: "BusinessClient");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Bzn",
                table: "BusinessClient");
        }
    }
}
