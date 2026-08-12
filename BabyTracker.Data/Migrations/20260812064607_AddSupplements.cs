using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplementDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BuiltInKey = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplementDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplementEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ChildId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplementEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplementDefinitionSupplementEntry",
                columns: table => new
                {
                    SupplementEntryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SupplementsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplementDefinitionSupplementEntry", x => new { x.SupplementEntryId, x.SupplementsId });
                    table.ForeignKey(
                        name: "FK_SupplementDefinitionSupplementEntry_SupplementDefinitions_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "SupplementDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplementDefinitionSupplementEntry_SupplementEntries_SupplementEntryId",
                        column: x => x.SupplementEntryId,
                        principalTable: "SupplementEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplementDefinitionSupplementEntry_SupplementsId",
                table: "SupplementDefinitionSupplementEntry",
                column: "SupplementsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplementDefinitionSupplementEntry");

            migrationBuilder.DropTable(
                name: "SupplementDefinitions");

            migrationBuilder.DropTable(
                name: "SupplementEntries");
        }
    }
}
