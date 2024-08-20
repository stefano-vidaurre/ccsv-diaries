using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCSV.Diaries.Contexts.Migrations.InFileApplicationContextMigrations
{
    /// <inheritdoc />
    public partial class AlterDiaryColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityDeletionDate",
                table: "CCSV_Diaries_Entries",
                newName: "EntityDisabledDate");

            migrationBuilder.RenameColumn(
                name: "EntityDeletionDate",
                table: "CCSV_Diaries_Diaries",
                newName: "EntityDisabledDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityDisabledDate",
                table: "CCSV_Diaries_Entries",
                newName: "EntityDeletionDate");

            migrationBuilder.RenameColumn(
                name: "EntityDisabledDate",
                table: "CCSV_Diaries_Diaries",
                newName: "EntityDeletionDate");
        }
    }
}
