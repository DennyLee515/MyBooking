using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBooking.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TouristRoutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 1500, nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<double>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    DepartureTime = table.Column<DateTime>(nullable: true),
                    Features = table.Column<string>(nullable: true),
                    Fees = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: true),
                    TravelDays = table.Column<int>(nullable: true),
                    TripType = table.Column<int>(nullable: true),
                    DepartureCity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TouristRoutePics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(maxLength: 100, nullable: true),
                    TouristRouteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristRoutePics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouristRoutePics_TouristRoutes_TouristRouteId",
                        column: x => x.TouristRouteId,
                        principalTable: "TouristRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouristRoutePics_TouristRouteId",
                table: "TouristRoutePics",
                column: "TouristRouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouristRoutePics");

            migrationBuilder.DropTable(
                name: "TouristRoutes");
        }
    }
}
