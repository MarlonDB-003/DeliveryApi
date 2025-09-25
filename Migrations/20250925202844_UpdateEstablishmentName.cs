using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace delivery.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEstablishmentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Establishments",
                newName: "EstablishmentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstablishmentName",
                table: "Establishments",
                newName: "Name");
        }
    }
}
