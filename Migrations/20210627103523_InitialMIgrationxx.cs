using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeCareWebApp.Migrations
{
    public partial class InitialMIgrationxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Bzn");

            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.CreateTable(
                name: "PartnerDtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    MomoCode = table.Column<string>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    DestinationLatitude = table.Column<string>(nullable: true),
                    DestinationLongitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PartnerServiceDispDtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    ServiceName = table.Column<string>(nullable: true),
                    PartnerId = table.Column<int>(nullable: false),
                    Partner = table.Column<string>(nullable: true),
                    ServiceImage = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PartnerServiceDtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PartnerId = table.Column<int>(nullable: false),
                    Partner = table.Column<string>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    Service = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Seats = table.Column<int>(nullable: true),
                    MinDuration = table.Column<int>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true),
                    DestinationLatitude = table.Column<string>(nullable: true),
                    DestinationLongitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ReservationDtos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    Client = table.Column<string>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true),
                    Partner = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    ReservationStatusId = table.Column<int>(nullable: true),
                    ReservationStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ScalarValueDtos",
                columns: table => new
                {
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserAuthenticationDtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Names = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    PartnerId = table.Column<int>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserDtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Names = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "BusinessClient",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 125, nullable: true),
                    SerialNumber = table.Column<string>(maxLength: 25, nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessService",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 125, nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerServiceImage",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerServiceImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationStatus",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 125, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 125, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientMovement",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    OriginLatitude = table.Column<string>(nullable: true),
                    OriginLongitude = table.Column<string>(nullable: true),
                    DestinationLatitude = table.Column<string>(nullable: true),
                    DestinationLongitude = table.Column<string>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientMovement_BusinessClient_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Bzn",
                        principalTable: "BusinessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 125, nullable: true),
                    Names = table.Column<string>(maxLength: 125, nullable: true),
                    Password = table.Column<string>(maxLength: 125, nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partner",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    Location = table.Column<string>(maxLength: 300, nullable: true),
                    Email = table.Column<string>(maxLength: 125, nullable: true),
                    Phone = table.Column<string>(maxLength: 10, nullable: true),
                    MomoCode = table.Column<string>(maxLength: 8, nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DestinationLatitude = table.Column<string>(nullable: true),
                    DestinationLongitude = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partner_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partner_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Barber",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Names = table.Column<string>(maxLength: 125, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 125, nullable: true),
                    PartnerId = table.Column<int>(nullable: true),
                    CreatedOnDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Barber_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Barber_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "Bzn",
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Barber_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartnerService",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PartnerId = table.Column<int>(nullable: false),
                    BusinessServiceId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Seats = table.Column<int>(nullable: true),
                    MinDuration = table.Column<int>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    CreatedOnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerService_BusinessService_BusinessServiceId",
                        column: x => x.BusinessServiceId,
                        principalSchema: "Bzn",
                        principalTable: "BusinessService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerService_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "Bzn",
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 11, nullable: false),
                    BusinessClientId = table.Column<string>(nullable: true),
                    PartnerId = table.Column<int>(nullable: true),
                    ReservationDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 6, nullable: true),
                    ReservationStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_BusinessClient_BusinessClientId",
                        column: x => x.BusinessClientId,
                        principalSchema: "Bzn",
                        principalTable: "BusinessClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservation_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "Bzn",
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservation_ReservationStatus_ReservationStatusId",
                        column: x => x.ReservationStatusId,
                        principalSchema: "Bzn",
                        principalTable: "ReservationStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservationDetail",
                schema: "Bzn",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 13, nullable: false),
                    ReservationId = table.Column<string>(nullable: true),
                    PartnerServiceId = table.Column<int>(nullable: false),
                    ReservationDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 6, nullable: true),
                    IsServed = table.Column<bool>(nullable: false),
                    BarberId = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationDetail_Barber_BarberId",
                        column: x => x.BarberId,
                        principalSchema: "Bzn",
                        principalTable: "Barber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationDetail_PartnerService_PartnerServiceId",
                        column: x => x.PartnerServiceId,
                        principalSchema: "Bzn",
                        principalTable: "PartnerService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationDetail_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalSchema: "Bzn",
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barber_CreatedById",
                schema: "Bzn",
                table: "Barber",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Barber_Email",
                schema: "Bzn",
                table: "Barber",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Barber_PartnerId",
                schema: "Bzn",
                table: "Barber",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Barber_Phone",
                schema: "Bzn",
                table: "Barber",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Barber_UserId",
                schema: "Bzn",
                table: "Barber",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessService_Name",
                schema: "Bzn",
                table: "BusinessService",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_CreatedById",
                schema: "Bzn",
                table: "Partner",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_Email",
                schema: "Bzn",
                table: "Partner",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_MomoCode",
                schema: "Bzn",
                table: "Partner",
                column: "MomoCode",
                unique: true,
                filter: "[MomoCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_Name",
                schema: "Bzn",
                table: "Partner",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_Phone",
                schema: "Bzn",
                table: "Partner",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Partner_UserId",
                schema: "Bzn",
                table: "Partner",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerService_BusinessServiceId",
                schema: "Bzn",
                table: "PartnerService",
                column: "BusinessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerService_PartnerId",
                schema: "Bzn",
                table: "PartnerService",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_BusinessClientId",
                schema: "Bzn",
                table: "Reservation",
                column: "BusinessClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_PartnerId",
                schema: "Bzn",
                table: "Reservation",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationStatusId",
                schema: "Bzn",
                table: "Reservation",
                column: "ReservationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDetail_BarberId",
                schema: "Bzn",
                table: "ReservationDetail",
                column: "BarberId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDetail_PartnerServiceId",
                schema: "Bzn",
                table: "ReservationDetail",
                column: "PartnerServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDetail_ReservationId",
                schema: "Bzn",
                table: "ReservationDetail",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMovement_ClientId",
                schema: "Security",
                table: "ClientMovement",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                schema: "Security",
                table: "Role",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: "Security",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                schema: "Security",
                table: "User",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerDtos");

            migrationBuilder.DropTable(
                name: "PartnerServiceDispDtos");

            migrationBuilder.DropTable(
                name: "PartnerServiceDtos");

            migrationBuilder.DropTable(
                name: "ReservationDtos");

            migrationBuilder.DropTable(
                name: "ScalarValueDtos");

            migrationBuilder.DropTable(
                name: "UserAuthenticationDtos");

            migrationBuilder.DropTable(
                name: "UserDtos");

            migrationBuilder.DropTable(
                name: "PartnerServiceImage",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "ReservationDetail",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "ClientMovement",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Barber",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "PartnerService",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "Reservation",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "BusinessService",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "BusinessClient",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "Partner",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "ReservationStatus",
                schema: "Bzn");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Security");
        }
    }
}
