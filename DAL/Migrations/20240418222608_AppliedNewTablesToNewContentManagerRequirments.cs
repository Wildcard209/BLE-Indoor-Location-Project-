using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AppliedNewTablesToNewContentManagerRequirments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HTMLPopups");

            migrationBuilder.CreateTable(
                name: "Css",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Css", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HTML",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HTML", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Javascript",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Javascript", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PopupLocation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    LocationX = table.Column<int>(type: "int", nullable: false),
                    LocationY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopupLocation", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Popups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    HtmlId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popups", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MapInfo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    CurrentCssId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CurrentJsId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CurrentHtmlId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MapInfo_Css_CurrentCssId",
                        column: x => x.CurrentCssId,
                        principalTable: "Css",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MapInfo_HTML_CurrentHtmlId",
                        column: x => x.CurrentHtmlId,
                        principalTable: "HTML",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MapInfo_Javascript_CurrentJsId",
                        column: x => x.CurrentJsId,
                        principalTable: "Javascript",
                        principalColumn: "ID");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MapPopups",
                columns: table => new
                {
                    PopupLocationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PopupId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapPopups", x => new { x.PopupLocationId, x.PopupId });
                    table.ForeignKey(
                        name: "FK_MapPopups_PopupLocation_PopupLocationId",
                        column: x => x.PopupLocationId,
                        principalTable: "PopupLocation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapPopups_Popups_PopupId",
                        column: x => x.PopupId,
                        principalTable: "Popups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MapInfo_CurrentCssId",
                table: "MapInfo",
                column: "CurrentCssId");

            migrationBuilder.CreateIndex(
                name: "IX_MapInfo_CurrentHtmlId",
                table: "MapInfo",
                column: "CurrentHtmlId");

            migrationBuilder.CreateIndex(
                name: "IX_MapInfo_CurrentJsId",
                table: "MapInfo",
                column: "CurrentJsId");

            migrationBuilder.CreateIndex(
                name: "IX_MapPopups_PopupId",
                table: "MapPopups",
                column: "PopupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapInfo");

            migrationBuilder.DropTable(
                name: "MapPopups");

            migrationBuilder.DropTable(
                name: "Css");

            migrationBuilder.DropTable(
                name: "HTML");

            migrationBuilder.DropTable(
                name: "Javascript");

            migrationBuilder.DropTable(
                name: "PopupLocation");

            migrationBuilder.DropTable(
                name: "Popups");

            migrationBuilder.CreateTable(
                name: "HTMLPopups",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HTMLPopups", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }
    }
}
