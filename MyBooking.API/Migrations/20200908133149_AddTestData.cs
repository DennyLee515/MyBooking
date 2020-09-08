using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBooking.API.Migrations
{
    public partial class AddTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TouristRoutes",
                columns: new[] { "Id", "CreateTime", "DepartureCity", "DepartureTime", "Description", "Discount", "Features", "Fees", "Note", "OriginalPrice", "Rating", "Title", "TravelDays", "TripType", "UpdateTime" },
                values: new object[] { new Guid("082bfbb9-d451-4b67-b3f5-cdda36779534"), new DateTime(2020, 9, 8, 13, 31, 49, 230, DateTimeKind.Utc).AddTicks(1317), null, null, "description", null, null, null, null, 0m, null, "TestTitle", null, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("082bfbb9-d451-4b67-b3f5-cdda36779534"));
        }
    }
}
