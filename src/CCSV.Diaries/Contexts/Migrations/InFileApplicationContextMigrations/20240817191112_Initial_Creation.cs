using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCSV.Diaries.Contexts.Migrations.InFileApplicationContextMigrations
{
    /// <inheritdoc />
    public partial class Initial_Creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CCSV_Diaries_Diaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityConcurrencyToken = table.Column<Guid>(type: "TEXT", nullable: false),
                    EntityCreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityEditionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityDeletionDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCSV_Diaries_Diaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CCSV_Diaries_Entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DiaryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    EntityConcurrencyToken = table.Column<Guid>(type: "TEXT", nullable: false),
                    EntityCreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityEditionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntityDeletionDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCSV_Diaries_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CCSV_Diaries_Entries_CCSV_Diaries_Diaries_DiaryId",
                        column: x => x.DiaryId,
                        principalTable: "CCSV_Diaries_Diaries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CCSV_Diaries_Entries_DiaryId",
                table: "CCSV_Diaries_Entries",
                column: "DiaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CCSV_Diaries_Entries");

            migrationBuilder.DropTable(
                name: "CCSV_Diaries_Diaries");
        }
    }
}
