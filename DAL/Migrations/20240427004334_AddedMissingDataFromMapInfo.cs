using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingDataFromMapInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapInfo_HTML_CurrentHtmlId",
                table: "MapInfo");

            migrationBuilder.DropIndex(
                name: "IX_MapInfo_CurrentHtmlId",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "CurrentHtmlId",
                table: "MapInfo");

            migrationBuilder.AddColumn<int>(
                name: "BoundX",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoundY",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultX",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultY",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HigherX",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HigherY",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageHeight",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageWidth",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowerX",
                table: "MapInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowerY",
                table: "MapInfo",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoundX",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "BoundY",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "DefaultX",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "DefaultY",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "HigherX",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "HigherY",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "ImageHeight",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "ImageWidth",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "LowerX",
                table: "MapInfo");

            migrationBuilder.DropColumn(
                name: "LowerY",
                table: "MapInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentHtmlId",
                table: "MapInfo",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapInfo_CurrentHtmlId",
                table: "MapInfo",
                column: "CurrentHtmlId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapInfo_HTML_CurrentHtmlId",
                table: "MapInfo",
                column: "CurrentHtmlId",
                principalTable: "HTML",
                principalColumn: "ID");
        }
    }
}
