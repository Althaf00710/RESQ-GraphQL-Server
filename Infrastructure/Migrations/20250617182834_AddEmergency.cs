using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmergency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CivilianStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilianStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FirstAidCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstAidCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RescueVehicleCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueVehicleCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SnakeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScientificName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VenomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnakeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Civilians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NicNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CivilianStatusId = table.Column<int>(type: "int", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Civilians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Civilians_CivilianStatuses_CivilianStatusId",
                        column: x => x.CivilianStatusId,
                        principalTable: "CivilianStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyToCivilians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivilianStatusId = table.Column<int>(type: "int", nullable: false),
                    EmergencyCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyToCivilians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyToCivilians_CivilianStatuses_CivilianStatusId",
                        column: x => x.CivilianStatusId,
                        principalTable: "CivilianStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmergencyToCivilians_EmergencyCategories_EmergencyCategoryId",
                        column: x => x.EmergencyCategoryId,
                        principalTable: "EmergencyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FirstAids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstAids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirstAids_FirstAidCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "FirstAidCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyToVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmergencyCategoryId = table.Column<int>(type: "int", nullable: false),
                    VehicleCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyToVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmergencyToVehicles_EmergencyCategories_EmergencyCategoryId",
                        column: x => x.EmergencyCategoryId,
                        principalTable: "EmergencyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmergencyToVehicles_RescueVehicleCategories_VehicleCategoryId",
                        column: x => x.VehicleCategoryId,
                        principalTable: "RescueVehicleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RescueVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RescueVehicleCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueVehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RescueVehicles_RescueVehicleCategories_RescueVehicleCategoryId",
                        column: x => x.RescueVehicleCategoryId,
                        principalTable: "RescueVehicleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CivilianLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivilianId = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilianLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CivilianLocations_Civilians_CivilianId",
                        column: x => x.CivilianId,
                        principalTable: "Civilians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CivilianStatusRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivilianStatusId = table.Column<int>(type: "int", nullable: false),
                    CivilianId = table.Column<int>(type: "int", nullable: false),
                    proofImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilianStatusRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CivilianStatusRequests_CivilianStatuses_CivilianStatusId",
                        column: x => x.CivilianStatusId,
                        principalTable: "CivilianStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CivilianStatusRequests_Civilians_CivilianId",
                        column: x => x.CivilianId,
                        principalTable: "Civilians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RescueVehicleRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CivilianId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    proofImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueVehicleRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RescueVehicleRequests_Civilians_CivilianId",
                        column: x => x.CivilianId,
                        principalTable: "Civilians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FirstAidDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstAidId = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstAidDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirstAidDetails_FirstAids_FirstAidId",
                        column: x => x.FirstAidId,
                        principalTable: "FirstAids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RescueVehicleLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RescueVehicleId = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueVehicleLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RescueVehicleLocations_RescueVehicles_RescueVehicleId",
                        column: x => x.RescueVehicleId,
                        principalTable: "RescueVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RescueVehicleAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RescueVehicleRequestId = table.Column<int>(type: "int", nullable: false),
                    RescueVehicleId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueVehicleAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RescueVehicleAssignments_RescueVehicleRequests_RescueVehicleRequestId",
                        column: x => x.RescueVehicleRequestId,
                        principalTable: "RescueVehicleRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RescueVehicleAssignments_RescueVehicles_RescueVehicleId",
                        column: x => x.RescueVehicleId,
                        principalTable: "RescueVehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CivilianLocations_CivilianId",
                table: "CivilianLocations",
                column: "CivilianId");

            migrationBuilder.CreateIndex(
                name: "IX_Civilians_CivilianStatusId",
                table: "Civilians",
                column: "CivilianStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CivilianStatusRequests_CivilianId",
                table: "CivilianStatusRequests",
                column: "CivilianId");

            migrationBuilder.CreateIndex(
                name: "IX_CivilianStatusRequests_CivilianStatusId",
                table: "CivilianStatusRequests",
                column: "CivilianStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyToCivilians_CivilianStatusId",
                table: "EmergencyToCivilians",
                column: "CivilianStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyToCivilians_EmergencyCategoryId",
                table: "EmergencyToCivilians",
                column: "EmergencyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyToVehicles_EmergencyCategoryId",
                table: "EmergencyToVehicles",
                column: "EmergencyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyToVehicles_VehicleCategoryId",
                table: "EmergencyToVehicles",
                column: "VehicleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FirstAidDetails_FirstAidId",
                table: "FirstAidDetails",
                column: "FirstAidId");

            migrationBuilder.CreateIndex(
                name: "IX_FirstAids_CategoryId",
                table: "FirstAids",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RescueVehicleAssignments_RescueVehicleId",
                table: "RescueVehicleAssignments",
                column: "RescueVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RescueVehicleAssignments_RescueVehicleRequestId",
                table: "RescueVehicleAssignments",
                column: "RescueVehicleRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RescueVehicleLocations_RescueVehicleId",
                table: "RescueVehicleLocations",
                column: "RescueVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RescueVehicleRequests_CivilianId",
                table: "RescueVehicleRequests",
                column: "CivilianId");

            migrationBuilder.CreateIndex(
                name: "IX_RescueVehicles_RescueVehicleCategoryId",
                table: "RescueVehicles",
                column: "RescueVehicleCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CivilianLocations");

            migrationBuilder.DropTable(
                name: "CivilianStatusRequests");

            migrationBuilder.DropTable(
                name: "EmergencyToCivilians");

            migrationBuilder.DropTable(
                name: "EmergencyToVehicles");

            migrationBuilder.DropTable(
                name: "FirstAidDetails");

            migrationBuilder.DropTable(
                name: "RescueVehicleAssignments");

            migrationBuilder.DropTable(
                name: "RescueVehicleLocations");

            migrationBuilder.DropTable(
                name: "SnakeTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EmergencyCategories");

            migrationBuilder.DropTable(
                name: "FirstAids");

            migrationBuilder.DropTable(
                name: "RescueVehicleRequests");

            migrationBuilder.DropTable(
                name: "RescueVehicles");

            migrationBuilder.DropTable(
                name: "FirstAidCategories");

            migrationBuilder.DropTable(
                name: "Civilians");

            migrationBuilder.DropTable(
                name: "RescueVehicleCategories");

            migrationBuilder.DropTable(
                name: "CivilianStatuses");
        }
    }
}
