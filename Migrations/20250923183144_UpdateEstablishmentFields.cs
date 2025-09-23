using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace delivery.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEstablishmentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Establishments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingTime",
                table: "Establishments",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Establishments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HasDeliveryPerson",
                table: "Establishments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumOrderValue",
                table: "Establishments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningTime",
                table: "Establishments",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_CategoryId",
                table: "Establishments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishments_Categories_CategoryId",
                table: "Establishments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishments_Categories_CategoryId",
                table: "Establishments");

            migrationBuilder.DropIndex(
                name: "IX_Establishments_CategoryId",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "HasDeliveryPerson",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "MinimumOrderValue",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Establishments");
        }
    }
}
