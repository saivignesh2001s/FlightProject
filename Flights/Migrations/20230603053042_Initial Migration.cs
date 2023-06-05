using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightDatas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    flightid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departure_destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    arrival_destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departure_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    arrival_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightDatas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightDatas");
        }
    }
}
