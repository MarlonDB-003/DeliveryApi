using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace delivery.Migrations
{
    /// <inheritdoc />
    public partial class InitialEstablishment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Restaurants_RestaurantId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Reviews",
                newName: "EstablishmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_RestaurantId",
                table: "Reviews",
                newName: "IX_Reviews_EstablishmentId");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Products",
                newName: "EstablishmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_RestaurantId",
                table: "Products",
                newName: "IX_Products_EstablishmentId");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Orders",
                newName: "EstablishmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                newName: "IX_Orders_EstablishmentId");

            migrationBuilder.CreateTable(
                name: "Establishments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Establishments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_UserId",
                table: "Establishments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Establishments_EstablishmentId",
                table: "Orders",
                column: "EstablishmentId",
                principalTable: "Establishments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Establishments_EstablishmentId",
                table: "Products",
                column: "EstablishmentId",
                principalTable: "Establishments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Establishments_EstablishmentId",
                table: "Reviews",
                column: "EstablishmentId",
                principalTable: "Establishments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Establishments_EstablishmentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Establishments_EstablishmentId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Establishments_EstablishmentId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Establishments");

            migrationBuilder.RenameColumn(
                name: "EstablishmentId",
                table: "Reviews",
                newName: "RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_EstablishmentId",
                table: "Reviews",
                newName: "IX_Reviews_RestaurantId");

            migrationBuilder.RenameColumn(
                name: "EstablishmentId",
                table: "Products",
                newName: "RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_EstablishmentId",
                table: "Products",
                newName: "IX_Products_RestaurantId");

            migrationBuilder.RenameColumn(
                name: "EstablishmentId",
                table: "Orders",
                newName: "RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_EstablishmentId",
                table: "Orders",
                newName: "IX_Orders_RestaurantId");

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_UserId",
                table: "Restaurants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Restaurants_RestaurantId",
                table: "Products",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
