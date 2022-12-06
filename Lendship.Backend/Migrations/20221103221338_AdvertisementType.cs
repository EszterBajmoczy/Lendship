using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class AdvertisementType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsService",
                table: "Advertisements");

            migrationBuilder.AddColumn<int>(
                name: "AdvertisementType",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertisementType",
                table: "Advertisements");

            migrationBuilder.AddColumn<bool>(
                name: "IsService",
                table: "Advertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
