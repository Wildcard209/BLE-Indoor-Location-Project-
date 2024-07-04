using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Beacons",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    MacAddress = table.Column<string>(type: "longtext", nullable: true),
                    UUID = table.Column<string>(type: "longtext", nullable: true),
                    Major = table.Column<string>(type: "longtext", nullable: true),
                    Minor = table.Column<string>(type: "longtext", nullable: true),
                    RSSI1M = table.Column<string>(type: "longtext", nullable: true),
                    RSSI = table.Column<string>(type: "longtext", nullable: true),
                    LocationX = table.Column<float>(type: "float", nullable: false),
                    LocationY = table.Column<float>(type: "float", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beacons", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beacons");
        }
    }
}
