using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class UpdateAdvertisement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ImageLocations_AdvertisementId",
                table: "ImageLocations",
                column: "AdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageLocations_Advertisements_AdvertisementId",
                table: "ImageLocations",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageLocations_Advertisements_AdvertisementId",
                table: "ImageLocations");

            migrationBuilder.DropIndex(
                name: "IX_ImageLocations_AdvertisementId",
                table: "ImageLocations");
        }
    }
}
