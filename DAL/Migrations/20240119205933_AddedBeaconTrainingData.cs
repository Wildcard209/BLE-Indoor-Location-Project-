using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedBeaconTrainingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeaconTrainingData",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    B1 = table.Column<int>(type: "int", nullable: true),
                    B2 = table.Column<int>(type: "int", nullable: true),
                    B3 = table.Column<int>(type: "int", nullable: true),
                    B4 = table.Column<int>(type: "int", nullable: true),
                    B5 = table.Column<int>(type: "int", nullable: true),
                    B6 = table.Column<int>(type: "int", nullable: true),
                    B7 = table.Column<int>(type: "int", nullable: true),
                    B8 = table.Column<int>(type: "int", nullable: true),
                    B9 = table.Column<int>(type: "int", nullable: true),
                    B10 = table.Column<int>(type: "int", nullable: true),
                    B11 = table.Column<int>(type: "int", nullable: true),
                    B12 = table.Column<int>(type: "int", nullable: true),
                    B13 = table.Column<int>(type: "int", nullable: true),
                    B14 = table.Column<int>(type: "int", nullable: true),
                    B15 = table.Column<int>(type: "int", nullable: true),
                    B16 = table.Column<int>(type: "int", nullable: true),
                    B17 = table.Column<int>(type: "int", nullable: true),
                    B18 = table.Column<int>(type: "int", nullable: true),
                    B19 = table.Column<int>(type: "int", nullable: true),
                    location = table.Column<string>(type: "longtext", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeaconTrainingData", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeaconTrainingData");
        }
    }
}
