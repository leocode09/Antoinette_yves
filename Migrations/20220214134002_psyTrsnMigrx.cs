using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeCareWebApp.Migrations
{
    public partial class psyTrsnMigrx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionStatus",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 125, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayTransaction",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReservationId = table.Column<string>(nullable: true),
                    ExternalReferenceNumber = table.Column<string>(maxLength: 125, nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: true),
                    TransactionStatusId = table.Column<int>(nullable: true),
                    CompletedOnDate = table.Column<DateTime>(nullable: true),
                    X_ReferenceId = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayTransaction_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalSchema: "Bzn",
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayTransaction_TransactionStatus_TransactionStatusId",
                        column: x => x.TransactionStatusId,
                        principalSchema: "Bzn",
                        principalTable: "TransactionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayTransaction_ReservationId",
                schema: "Bzn",
                table: "PayTransaction",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayTransaction_TransactionStatusId",
                schema: "Bzn",
                table: "PayTransaction",
                column: "TransactionStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayTransaction",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "TransactionStatus",
                schema: "Bzn");
        }
    }
}
